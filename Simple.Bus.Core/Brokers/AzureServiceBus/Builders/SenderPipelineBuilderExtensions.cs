using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.AzureServiceBus.Builders
{
    public static class SenderPipelineBuilderExtensions
    {
        public static SenderPipelineBuilderFor<T> UseAzureServiceBus<T>(this SenderPipelineBuilderFor<T> builder, string connectionString, string topic)
        {
            builder.UseSender((logger) => new SenderAzureServiceBusFor<T>(connectionString, topic, logger));
            return builder;
        }
    }
}
