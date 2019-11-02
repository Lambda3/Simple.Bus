using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.RabbitMQ.Builders
{
    public static class ReceiverBuilderExtensions
    {
        public static ReceiverBuilderFor<T> WithRabbitMQ<T>(this ReceiverBuilderFor<T> receiver, ReceiverConfigurationRabbitMQ configuration)
        {
            receiver.WithReceiver((pipeline, logger) => new ReceiverRabbitMQFor<T>(pipeline, new ResourcesRabbitMQ(), configuration, logger));
            return receiver;
        }
    }
}
