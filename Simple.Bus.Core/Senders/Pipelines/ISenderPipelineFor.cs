using System.Threading.Tasks;

namespace Simple.Bus.Core.Senders.Pipelines
{
    public interface ISenderPipelineFor<T>
    {
        Task SendAsync(T message);
    }
}
