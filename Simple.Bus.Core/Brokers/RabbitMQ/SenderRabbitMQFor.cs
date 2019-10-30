using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Simple.Bus.Core.Senders;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public class SenderRabbitMQFor<T> : ISenderFor<T>, IDisposable
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string exchange;
        private readonly ILogger logger;
        private const string RoutingKey = "";

        public SenderRabbitMQFor(CredentialsRabbitMQ credentials, string exchange, ILogger logger)
        {
            this.exchange = exchange;
            this.logger = logger;
            var factory = credentials.Get();

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
        }

        public Task SendAsync(byte[] message)
        {
            logger.LogInformation($"Sending message to rabbitMQ. Exchange: {exchange}");
            channel.BasicPublish(exchange: exchange, routingKey: RoutingKey, body: message);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            logger.LogInformation("Closing connection");
            channel.Dispose();
            connection.Dispose();
        }
    }
}
