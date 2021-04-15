using RabbitMQ.Client;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public class ResourcesRabbitMQ
    {
        const string RoutingKey = "";

        public void CreateIfNotExist<T>(IModel channel, ReceiverConfigurationRabbitMQ<T> receiverConfiguration)
        {
            channel.ExchangeDeclare(exchange: receiverConfiguration.Exchange, type: receiverConfiguration.TypeForExchange, durable: receiverConfiguration.Queue.Durable);
            channel.QueueDeclare(queue: receiverConfiguration.Queue.Name, durable: receiverConfiguration.Queue.Durable, exclusive: receiverConfiguration.Queue.Exclusive, autoDelete: receiverConfiguration.Queue.AutoDelete);
            channel.QueueBind(receiverConfiguration.Queue.Name, receiverConfiguration.Exchange, RoutingKey);
        }
    }
}
