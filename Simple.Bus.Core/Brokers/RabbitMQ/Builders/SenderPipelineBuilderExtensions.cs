using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.RabbitMQ.Builders
{
    public static class SenderPipelineBuilderExtensions
    {
        public static SenderPipelineBuilderFor<T> UseRabbitMq<T>(this SenderPipelineBuilderFor<T> builder, CredentialsRabbitMQ credentials, string exchange)
        {
            builder.UseSender((logger) => new SenderRabbitMQFor<T>(credentials, exchange, logger));
            return builder;
        }
    }
}
