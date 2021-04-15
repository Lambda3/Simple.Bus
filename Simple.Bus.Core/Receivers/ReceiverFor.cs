using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers.Pipelines;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers
{
    public abstract class ReceiverFor<T> : IReceiverFor<T>
    {
        private readonly Func<IPipelineReceiverFor<T>> services;
        protected readonly ILogger<IReceiverFor<T>> logger;

        public ReceiverFor(Func<IPipelineReceiverFor<T>> services, ILogger<IReceiverFor<T>> logger)
        {
            this.services = services;
            this.logger = logger;
        }

        protected Task ExecutePipelineAsync(ReadOnlySpan<byte> message)
        {
            logger.LogInformation("Executing receiver pipeline");
            try
            {
                using var pipeline = services();
                return pipeline.Receive(Encoding.UTF8.GetString(message));
            }
            catch (Exception e)
            {
                logger.LogError($"Error while executing pipeline. {e.Message}", e);
                throw;
            }
        }

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public abstract bool IsConnected();
    }
}
