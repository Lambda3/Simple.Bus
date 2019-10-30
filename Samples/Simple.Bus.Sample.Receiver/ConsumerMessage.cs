using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Sample.Receiver
{
    public class ConsumerMessage 
    {
        private readonly ILogger logger;

        public ConsumerMessage(ILogger logger)
        {
            this.logger = logger;
        }

        public Task Consume(MessageContractASB message)
        {
            logger.LogInformation($"Receive message for azure service bus: {message.Nome}");
            return Task.CompletedTask;
        }
    }
}
