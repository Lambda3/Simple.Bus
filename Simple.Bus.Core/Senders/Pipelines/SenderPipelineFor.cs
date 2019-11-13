using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Serializers;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Senders.Pipelines
{
    public class SenderPipelineFor<T> : ISenderPipelineFor<T>
    {
        private readonly ISenderFor<T> sender;
        private readonly ICryptography cryptography;
        private readonly ISerializer serializer;
        private readonly ILogger logger;

        public SenderPipelineFor(ISenderFor<T> sender, ICryptography cryptography, ISerializer serializer, ILogger<ISenderPipelineFor<T>> logger)
        {
            this.sender = sender;
            this.cryptography = cryptography;
            this.serializer = serializer;
            this.logger = logger;
        }

        public Task SendAsync(T message)
        {
            logger.LogInformation("Serializing message");
            var messageSerialized = serializer.Serialize(message);
            logger.LogInformation("Encrypting message");
            var messageCriptographed = cryptography.Encrypt(messageSerialized);
            var messageBytes = Encoding.UTF8.GetBytes(messageCriptographed);
            logger.LogInformation("Sending message");
            return sender.SendAsync(messageBytes);
        }
    }
}
