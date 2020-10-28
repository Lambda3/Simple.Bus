using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers.Pipelines
{
    public interface IPipelineReceiverFor<T> : IDisposable
    {
        Action EventoDispose { get; set; }

        Task Receive(string message);
    }
}
