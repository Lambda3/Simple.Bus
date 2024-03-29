﻿using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Simple.Bus.Core.Receivers;
using Simple.Bus.Core.Receivers.Pipelines;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public partial class ReceiverRabbitMQFor<T> : ReceiverFor<T>
    {
        private readonly ResourcesRabbitMQ resources;
        private readonly ReceiverConfigurationRabbitMQ<T> receiverConfiguration;
        private readonly CredentialsRabbitMQ credentials;
        private IConnection connection;
        private IModel channel;

        public ReceiverRabbitMQFor(Func<IPipelineReceiverFor<T>> services,
            ResourcesRabbitMQ resources,
            ReceiverConfigurationRabbitMQ<T> receiverConfiguration,
            CredentialsRabbitMQ credentials,
            ILogger<IReceiverFor<T>> logger) : base(services, logger)
        {
            this.resources = resources;
            this.receiverConfiguration = receiverConfiguration;
            this.credentials = credentials;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = credentials.Get();
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            resources.CreateIfNotExist(channel, receiverConfiguration);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += ProccessMessageAsync;

            channel.BasicConsume(receiverConfiguration.Queue.Name, receiverConfiguration.AutoCompleteMessage, consumer);

            return Task.CompletedTask;
        }

        async Task ProccessMessageAsync(object message, BasicDeliverEventArgs e)
        {
            await ExecutePipelineAsync(e.Body.Span);

            if (!receiverConfiguration.AutoCompleteMessage)
                channel.BasicAck(e.DeliveryTag, false);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Dispose();
            connection.Dispose();
            return Task.CompletedTask;
        }

        public override bool IsConnected() => connection.IsOpen;
    }
}
