using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;

namespace Simple.Bus.Receiver.DeadLetter
{
    public class Worker : IHostedService
    {
        private readonly ILogger<Worker> logger;
        private readonly IReceiverFor<MessageErrorContract> handleReceiver;

        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }

        public Worker(ILogger<Worker> logger, IReceiverFor<MessageErrorContract> handleReceiver)
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
