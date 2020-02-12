using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Receivers;
using Simple.Bus.Core.Receivers.Pipelines;
using Simple.Bus.Core.Serializers;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Builders
{
    public class ReceiverBuilderFor<T>
    {
        private Func<IPipelineReceiverFor<T>> pipeline;
        private ISerializer serializer;
        private ICryptography cryptography;
        private Func<Func<IPipelineReceiverFor<T>>, ILogger<IReceiverFor<T>>, IReceiverFor<T>> receiver;
        private IConsumerFor<T> handlerMessageFunction;
        private ILogger<IPipelineReceiverFor<T>> loggerPipeline;
        private ILogger<IReceiverFor<T>> loggerReceiver;

        public ReceiverBuilderFor<T> WithPipeline(Func<IPipelineReceiverFor<T>> pipeline)
        {
            this.pipeline = pipeline;
            return this;
        }

        public ReceiverBuilderFor<T> WithSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }

        public ReceiverBuilderFor<T> WithCriptographer(ICryptography cryptography)
        {
            this.cryptography = cryptography;
            return this;
        }

        public ReceiverBuilderFor<T> WithMessageHandler(Func<T, Task> handlerMessageFunction)
        {
            this.handlerMessageFunction = new LambdaConsumer<T>(handlerMessageFunction);
            return this;
        }
        public ReceiverBuilderFor<T> WithMessageHandler(IConsumerFor<T> consumer)
        {
            handlerMessageFunction = consumer;
            return this;
        }

        public ReceiverBuilderFor<T> WithReceiver(Func<Func<IPipelineReceiverFor<T>>, ILogger<IReceiverFor<T>>, IReceiverFor<T>> receiver)
        {
            this.receiver = receiver;
            return this;
        }

        public ReceiverBuilderFor<T> WithLogger(ILogger<IPipelineReceiverFor<T>> logger)
        {
            loggerPipeline = logger;
            return this;
        }
        public ReceiverBuilderFor<T> WithLogger(ILogger<IReceiverFor<T>> logger)
        {
            loggerReceiver = logger;
            return this;
        }


        public IReceiverFor<T> Build()
        {
            if (serializer == null)
                WithSerializer(new SerializerDefault());

            if (cryptography == null)
                WithCriptographer(new CryptographyDefault());

            if (pipeline == null)
                WithPipeline(() => new PipelineReceiverFor<T>(handlerMessageFunction, serializer, cryptography, loggerPipeline));

            if (handlerMessageFunction == null)
                throw new ArgumentNullException(nameof(handlerMessageFunction), "Must be specified a handler message function");

            if (loggerPipeline == null)
                throw new ArgumentNullException(nameof(loggerPipeline));

            if (loggerReceiver == null)
                throw new ArgumentNullException(nameof(loggerReceiver));

            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver), "Must be specified a transport");

            return receiver(pipeline, loggerReceiver);
        }
    }
}
