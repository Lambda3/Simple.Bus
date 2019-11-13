using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers
{
    public abstract class ReceiverFor<T> : IReceiverFor<T>
    {
        private readonly IPipelineReceiverFor<T> pipeline;
        protected readonly ILogger<IReceiverFor<T>> logger;

        public ReceiverFor(IPipelineReceiverFor<T> pipeline, ILogger<IReceiverFor<T>> logger)
        {
            this.pipeline = pipeline;
            this.logger = logger;
        }

        protected Task ExecutePipelineAsync(byte[] message)
        {
            logger.LogInformation("Executing receiver pipeline");
            return pipeline.Receive(Encoding.UTF8.GetString(message));
        }

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public abstract bool IsConnected();
    }
}
