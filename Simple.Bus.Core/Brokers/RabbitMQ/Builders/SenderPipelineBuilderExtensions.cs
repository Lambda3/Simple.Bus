using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.RabbitMQ.Builders
{
    public static class SenderPipelineBuilderExtensions
    {
        public static SenderPipelineBuilderFor<T> WithRabbitMq<T>(this SenderPipelineBuilderFor<T> builder, CredentialsRabbitMQ credentials, string exchange)
        {
            builder.WithSender((logger) => new SenderRabbitMQFor<T>(credentials, exchange, logger));
            return builder;
        }
    }
}
