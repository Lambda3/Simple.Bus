using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers.Pipelines
{
    public interface IPipelineReceiverFor<T>
    {
        Task Receive(string message);
    }
}
