using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;
using System.Threading.Tasks;

namespace Simple.Bus.Sample.Receiver
{
    public class ConsumerMessageRMQ: IConsumerFor<MessageContractRMQ>
    {
        private readonly ILogger<ConsumerMessage> logger;

        public ConsumerMessageRMQ(ILogger<ConsumerMessage> logger)
        {
            this.logger = logger;
        }

        public Task Consume(MessageContractRMQ message)
        {
            logger.LogInformation($"Receive message for rabbitmq: {message.Nome}");
            return Task.CompletedTask;
        }
    }
}
