using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Serializers;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers.Pipelines
{
    public class PipelineReceiverFor<T> : IPipelineReceiverFor<T>
    {
        private readonly IConsumerFor<T> consumer;
        private readonly ISerializer serializer;
        private readonly ICryptography cryptography;
        private readonly ILogger<IPipelineReceiverFor<T>> logger;

        public PipelineReceiverFor(IConsumerFor<T> consumer, ISerializer serializer, ICryptography cryptography, ILogger<IPipelineReceiverFor<T>> logger)
        {
            this.consumer = consumer;
            this.serializer = serializer;
            this.cryptography = cryptography;
            this.logger = logger;
        }

        public Task Receive(string message)
        {
            logger.LogInformation("Decrypting message");
            var messageDecrypted = cryptography.Decrypt(message);
            logger.LogInformation("Deserializing message");
            var value = serializer.Deserialize<T>(messageDecrypted);
            logger.LogInformation("Executing callback");
            return consumer.Consume(value);
        }
    }
}
