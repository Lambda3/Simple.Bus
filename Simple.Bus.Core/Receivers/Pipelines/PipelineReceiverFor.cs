using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Serializers;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers.Pipelines
{
    public class PipelineReceiverFor<T> : IPipelineReceiverFor<T>
    {
        private readonly Func<T, Task> functionHandler;
        private readonly ISerializer serializer;
        private readonly ICryptography cryptography;
        private readonly ILogger logger;

        public PipelineReceiverFor(Func<T, Task> functionHandler, ISerializer serializer, ICryptography cryptography, ILogger logger)
        {
            this.functionHandler = functionHandler;
            this.functionHandler = functionHandler;
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
            return functionHandler.Invoke(value);
        }
    }
}
