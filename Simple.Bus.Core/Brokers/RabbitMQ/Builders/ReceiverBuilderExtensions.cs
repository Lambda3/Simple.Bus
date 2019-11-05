using Simple.Bus.Core.Builders;

namespace Simple.Bus.Core.Brokers.RabbitMQ.Builders
{
    public static class ReceiverBuilderExtensions
    {
        public static ReceiverBuilderFor<T> WithRabbitMQ<T>(this ReceiverBuilderFor<T> receiver, ReceiverConfigurationRabbitMQ<T> configuration, CredentialsRabbitMQ credentials)
        {
            receiver.WithReceiver((pipeline, logger) => new ReceiverRabbitMQFor<T>(pipeline, new ResourcesRabbitMQ(), configuration, credentials, logger));
            return receiver;
        }
    }
}
