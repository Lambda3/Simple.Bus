using System.Threading.Tasks;

namespace Simple.Bus.Core.Senders
{
    public interface ISenderFor<T>
    {
        Task SendAsync(byte[] message);
    }
}
