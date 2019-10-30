using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;

namespace Simple.Bus.Sample.Receiver
{
    public class WorkerRabbitMQ : IHostedService
    {
        private readonly ILogger<WorkerRabbitMQ> logger;
        private readonly IReceiverFor<MessageContractRMQ> handleReceiver;

        public WorkerRabbitMQ(ILogger<WorkerRabbitMQ> logger, IReceiverFor<MessageContractRMQ> handleReceiver)
        {
            this.logger = logger;
            this.handleReceiver = handleReceiver;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Start listening bus at {DateTimeOffset.Now}");
            return handleReceiver.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stop listening bus at {DateTimeOffset.Now}");
            return handleReceiver.StopAsync(cancellationToken);
        }
    }
}
