using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;
using System.Threading.Tasks;

namespace Simple.Bus.Sample.Receiver
{
    public class ConsumerMessage : IConsumerFor<MessageContractASB>
    {
        private readonly ILogger<ConsumerMessage> logger;

        public ConsumerMessage(ILogger<ConsumerMessage> logger)
        {
            this.logger = logger;
        }

        public Task Consume(MessageContractASB message)
        {
            logger.LogInformation($"Receive message for azure service bus: {message.Nome}");
            //throw new RetryException("Custom error");
            return Task.CompletedTask;
        }
    }
}
