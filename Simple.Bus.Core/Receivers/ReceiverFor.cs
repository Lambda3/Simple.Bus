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

        protected Task ExecutePipelineAsync(byte[] message)
        {
            logger.LogInformation("Executing receiver pipeline");
            IPipelineReceiverFor<T> pipeline;
            try
            {
                pipeline = services.Invoke();
            }
            catch (Exception e)
            {
                logger.LogError($"Error while creating pipeline. {e.Message}", e);
                throw;
            }

            return pipeline.Receive(Encoding.UTF8.GetString(message));
        }

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public abstract bool IsConnected();
    }
}
