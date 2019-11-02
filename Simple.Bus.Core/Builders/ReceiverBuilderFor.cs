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
        private IPipelineReceiverFor<T> pipeline;
        private ISerializer serializer;
        private ICryptography cryptography;
        private Func<IPipelineReceiverFor<T>, ILogger, IReceiverFor<T>> receiver;
        private Func<T, Task> handlerMessageFunction;
        private ILogger logger;

        public ReceiverBuilderFor<T> WithPipeline(IPipelineReceiverFor<T> pipeline)
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
            this.handlerMessageFunction = handlerMessageFunction;
            return this;
        }
        public ReceiverBuilderFor<T> WithMessageHandler(IConsumerFor<T> consumer)
        {
            handlerMessageFunction = consumer.Consume;
            return this;
        }

        public ReceiverBuilderFor<T> WithReceiver(Func<IPipelineReceiverFor<T>, ILogger, IReceiverFor<T>> receiver)
        {
            this.receiver = receiver;
            return this;
        }

        public ReceiverBuilderFor<T> WithLogger(ILogger logger)
        {
            this.logger = logger;
            return this;
        }

        public IReceiverFor<T> Build()
        {
            if (serializer == null)
                WithSerializer(new SerializerDefault());

            if (cryptography == null)
                WithCriptographer(new CryptographyDefault());

            if (logger == null)
                WithLogger(Loggers.LoggerFactory.CreateLogger<T>());

            if (pipeline == null)
                WithPipeline(new PipelineReceiverFor<T>(handlerMessageFunction, serializer, cryptography, logger));

            if (handlerMessageFunction == null)
                throw new ArgumentNullException(nameof(handlerMessageFunction), "Must be specified a handler message function");

            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver), "Must be specified a transport");

            return receiver.Invoke(pipeline, logger);
        }
    }
}
