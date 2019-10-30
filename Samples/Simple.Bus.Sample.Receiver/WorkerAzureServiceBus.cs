using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;

namespace Simple.Bus.Sample.Receiver
{
    public class WorkerAzureServiceBus : IHostedService
    {
        private readonly ILogger<WorkerAzureServiceBus> logger;
        private readonly IReceiverFor<MessageContractASB> handleReceiver;

        public WorkerAzureServiceBus(ILogger<WorkerAzureServiceBus> logger, IReceiverFor<MessageContractASB> handleReceiver)
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
