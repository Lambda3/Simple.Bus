using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.AzureServiceBus.Builders
{
    public static class ReceiverBuilderExtensions
    {
        public static ReceiverBuilderFor<T> WithAzureServiceBus<T>(this ReceiverBuilderFor<T> receiver, ReceiverConfigurationAzureServiceBus configuration)
        {
            receiver.WithReceiver((pipeline, logger) => new ReceiverAzureServiceBusFor<T>(pipeline, configuration, logger));
            return receiver;
        }
    }
}
