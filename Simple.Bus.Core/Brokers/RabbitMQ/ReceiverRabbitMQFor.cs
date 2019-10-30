﻿using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Simple.Bus.Core.Receivers;
using Simple.Bus.Core.Receivers.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public partial class ReceiverRabbitMQFor<T> : ReceiverFor<T>
    {
        private readonly ResourcesRabbitMQ resources;
        private readonly ReceiverConfigurationRabbitMQ receiverConfiguration;
        private IConnection connection;
        private IModel channel;

        public ReceiverRabbitMQFor(IPipelineReceiverFor<T> pipeline, ResourcesRabbitMQ resources, ReceiverConfigurationRabbitMQ receiverConfiguration, ILogger logger) : base(pipeline, logger)
        {
            this.resources = resources;
            this.receiverConfiguration = receiverConfiguration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = receiverConfiguration.Credentials.Get();
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            resources.CreateIfNotExist(channel, receiverConfiguration);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ProccessMessageAsync;

            channel.BasicConsume(receiverConfiguration.Queue.Name, receiverConfiguration.AutoCompleteMessage, consumer);

            return Task.CompletedTask;
        }

        void ProccessMessageAsync(object message, BasicDeliverEventArgs e)
        {
            ExecutePipelineAsync(e.Body).GetAwaiter().GetResult();

            if (!receiverConfiguration.AutoCompleteMessage)
                channel.BasicAck(e.DeliveryTag, false);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Dispose();
            connection.Dispose();
            return Task.CompletedTask;
        }
    }
}