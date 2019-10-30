﻿using RabbitMQ.Client;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public class ResourcesRabbitMQ
    {
        const string RoutingKey = "";

        public void CreateIfNotExist(IModel channel, ReceiverConfigurationRabbitMQ receiverConfiguration)
        {
            channel.ExchangeDeclare(exchange: receiverConfiguration.Exchange, type: receiverConfiguration.TypeForExchange);
            channel.QueueDeclare(queue: receiverConfiguration.Queue.Name, durable: receiverConfiguration.Queue.Durable, exclusive: receiverConfiguration.Queue.Exclusive, autoDelete: receiverConfiguration.Queue.AutoDelete);
            channel.QueueBind(receiverConfiguration.Queue.Name, receiverConfiguration.Exchange, RoutingKey);
        }
    }
}