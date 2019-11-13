using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Senders;
using Simple.Bus.Core.Senders.Pipelines;
using Simple.Bus.Core.Serializers;
using System;

namespace Simple.Bus.Core.Builders
{
    public class SenderPipelineBuilderFor<T>
    {
        private ISerializer serializer;
        private ICryptography cryptography;
        private Func<ILogger<ISenderFor<T>>, ISenderFor<T>> sender;
        public ILogger<ISenderPipelineFor<T>> loggerPipeline;
        public ILogger<ISenderFor<T>> loggerSender;

        public SenderPipelineBuilderFor<T> WithSerializer(ISerializer serializer)
        {
            this.serializer = serializer;
            return this;
        }

        public SenderPipelineBuilderFor<T> WithCriptographer(ICryptography cryptography)
        {
            this.cryptography = cryptography;
            return this;
        }

        public SenderPipelineBuilderFor<T> WithSender(Func<ILogger<ISenderFor<T>>, ISenderFor<T>> sender)
        {
            this.sender = sender;
            return this;
        }
        public SenderPipelineBuilderFor<T> WithLogger(ILogger<ISenderPipelineFor<T>> logger)
        {
            loggerPipeline = logger;
            return this;
        }
        public SenderPipelineBuilderFor<T> WithLogger(ILogger<ISenderFor<T>> logger)
        {
            loggerSender = logger;
            return this;
        }


        public ISenderPipelineFor<T> Build()
        {
            if (serializer == null)
                WithSerializer(new SerializerDefault());

            if (cryptography == null)
                WithCriptographer(new CryptographyDefault());

            if (loggerPipeline == null)
                throw new ArgumentNullException(nameof(loggerPipeline));

            if (loggerSender == null)
                throw new ArgumentNullException(nameof(loggerSender));

            if (sender == null)
                throw new ArgumentNullException(nameof(sender), "Sender transport must be especified.");

            return new SenderPipelineFor<T>(sender.Invoke(loggerSender), cryptography, serializer, loggerPipeline);
        }
    }
}
