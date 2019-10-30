using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers
{
    public interface IConsumerFor<T>
    {
        Task Consume(T message);
    }
}
