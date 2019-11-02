using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.AzureServiceBus.Builders
{
    public static class SenderPipelineBuilderExtensions
    {
        public static SenderPipelineBuilderFor<T> WithAzureServiceBus<T>(this SenderPipelineBuilderFor<T> builder, string connectionString, string topic)
        {
            builder.WithSender((logger) => new SenderAzureServiceBusFor<T>(connectionString, topic, logger));
            return builder;
        }
    }
}
