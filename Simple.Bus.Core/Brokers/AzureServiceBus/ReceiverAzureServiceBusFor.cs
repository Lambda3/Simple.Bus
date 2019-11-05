using Microsoft.Azure.Amqp;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;
using Simple.Bus.Core.Receivers.Pipelines;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Brokers.AzureServiceBus
{
    public class ReceiverAzureServiceBusFor<T> : ReceiverFor<T>
    {
        private readonly ISubscriptionClient subscriptionClient;
        private readonly ReceiverConfigurationAzureServiceBus<T> receiverConfiguration;

        public ReceiverAzureServiceBusFor(IPipelineReceiverFor<T> pipeline,
            ReceiverConfigurationAzureServiceBus<T> receiverConfiguration, 
            CredentialsAzureServiceBus credentials,
            ILogger logger) : base(pipeline, logger)
        {
            subscriptionClient = new SubscriptionClient(credentials.Get(), receiverConfiguration.TopicName, receiverConfiguration.SubscriptionName)
            {
                PrefetchCount = 10,
                OperationTimeout = TimeSpan.FromMinutes(1)
            };

            this.receiverConfiguration = receiverConfiguration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandlerAsync)
            {
                MaxConcurrentCalls = receiverConfiguration.MaxConcorrentCalls,
                AutoComplete = receiverConfiguration.AutoCompleteMessage,
                MaxAutoRenewDuration = TimeSpan.FromMinutes(10)
            };

            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            return Task.CompletedTask;
        }

        Task ExceptionReceivedHandlerAsync(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            logger.LogError($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            return Task.CompletedTask;
        }

        async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            logger.LogInformation($"Enqueued time: {message.SystemProperties.EnqueuedTimeUtc}");
            logger.LogInformation($"Lock until {message.SystemProperties.LockedUntilUtc}");
            await ExecutePipelineAsync(message.Body);
            if (!cancellationToken.IsCancellationRequested)
            {
                await CompleteMesageManually(message);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken) =>
            cancellationToken.IsCancellationRequested ? Task.CompletedTask : subscriptionClient.CloseAsync();

        private Task CompleteMesageManually(Message message)
        {
            if (receiverConfiguration.AutoCompleteMessage)
            {
                logger.LogInformation("Auto completed message");
                return Task.CompletedTask;
            }
            else
            {
                logger.LogInformation("Message completed manually");
                return subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }

        public override bool IsConnected()
        {
            var connectionManager = (FaultTolerantAmqpObject<AmqpConnection>)typeof(ServiceBusConnection)
                .GetProperty("ConnectionManager", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetMethod
                .Invoke(subscriptionClient.ServiceBusConnection, null);

            try
            {
                return connectionManager.TryGetOpenedObject(out _);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error while checking connection");
                return false;
            }
        }
    }
}
