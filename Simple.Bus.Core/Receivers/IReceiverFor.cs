using System.Threading;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers
{
    public interface IReceiverFor<T>
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        bool IsConnected();
    }
}
