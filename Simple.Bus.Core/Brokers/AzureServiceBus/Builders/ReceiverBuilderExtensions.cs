using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.AzureServiceBus.Builders
{
    public static class ReceiverBuilderExtensions
    {
        public static ReceiverBuilderFor<T> UseAzureServiceBus<T>(this ReceiverBuilderFor<T> receiver, ReceiverConfigurationAzureServiceBus configuration)
        {
            receiver.UseReceiver((pipeline, logger) => new ReceiverAzureServiceBusFor<T>(pipeline, configuration, logger));
            return receiver;
        }
    }
}
