using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Senders;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Brokers.AzureServiceBus
{
    public class SenderAzureServiceBusFor<T> : ISenderFor<T>, IDisposable
    {
        private readonly ITopicClient topicClient;
        private readonly ILogger logger;

        public SenderAzureServiceBusFor(string connectionString, string topic, ILogger logger)
        {
            topicClient = new TopicClient(connectionString, topic)
            {
                OperationTimeout = TimeSpan.FromSeconds(20)
            };
            this.logger = logger;
        }

        public Task SendAsync(byte[] message)
        {
            logger.LogInformation($"Sending message to azure service bus. Topic: {topicClient.TopicName}");
            return topicClient.SendAsync(new Message(message));
        }
        public void Dispose()
        {
            logger.LogInformation("Closing connection");
            topicClient.CloseAsync();
        }
    }
}
