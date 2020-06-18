using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;

namespace Simple.Bus.Sample.Receiver
{
    public class WorkerAzureServiceBus : BackgroundService
    {
        private readonly ILogger<WorkerAzureServiceBus> logger;
        private readonly IReceiverFor<MessageContractASB> handleReceiver;

        public WorkerAzureServiceBus(ILogger<WorkerAzureServiceBus> logger, IReceiverFor<MessageContractASB> handleReceiver)
        {
            this.logger = logger;
            this.handleReceiver = handleReceiver;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Start listening bus at {DateTimeOffset.Now}");
            await handleReceiver.StartAsync(cancellationToken);
            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Stop listening bus at {DateTimeOffset.Now}");
            await handleReceiver.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var message = handleReceiver.IsConnected() ? $"Alive at" : "Not alive at";
                //logger.LogInformation($"Azure Service Bus. {message} {DateTimeOffset.Now}");

                try
                {
                    await Task.Delay(5000, stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }
    }
}
