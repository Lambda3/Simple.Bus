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
        private readonly CredentialsAzureServiceBus credentials;

        public ReceiverAzureServiceBusFor(Func<IPipelineReceiverFor<T>> services,
            ReceiverConfigurationAzureServiceBus<T> receiverConfiguration,
            CredentialsAzureServiceBus credentials,
            ILogger<IReceiverFor<T>> logger) : base(services, logger)
        {
            subscriptionClient = new SubscriptionClient(credentials.Get(), receiverConfiguration.TopicName, receiverConfiguration.SubscriptionName)
            {
                PrefetchCount = 10,
                OperationTimeout = TimeSpan.FromMinutes(1)
            };

            this.receiverConfiguration = receiverConfiguration;
            this.credentials = credentials;
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

            try
            {
                await ExecutePipelineAsync(message.Body);
            }
            catch (RetryException e)
            {
                await CustomRetry(message, e);
            }

            if (!cancellationToken.IsCancellationRequested)
            {
                await CompleteMesageManually(message);
            }
        }

        private async Task CustomRetry(Message message, Exception e)
        {
            const string keyCount = "count_retry";
            var newMessage = message.Clone();
            var counter = 1;
            if (message.UserProperties.ContainsKey(keyCount))
            {
                int.TryParse(message.UserProperties[keyCount].ToString(), out counter);
                counter++;
                newMessage.UserProperties[keyCount] = counter;
            }
            else
                newMessage.UserProperties.Add(keyCount, counter);

            if (counter > receiverConfiguration.CustomRetryCount)
                throw new ServiceBusException(false, "Max custom retry reached", e);

            logger.LogError($"Retry exception");
            logger.LogInformation($"Attemped retry {counter} - {receiverConfiguration.CustomRetryCount}");
            newMessage.ScheduledEnqueueTimeUtc = message.SystemProperties.EnqueuedTimeUtc.AddSeconds(receiverConfiguration.CustomRetryDelay);
            logger.LogInformation($"Retry at {newMessage.ScheduledEnqueueTimeUtc}");
            var topicClient = new TopicClient(credentials.Get(), receiverConfiguration.TopicName);
            await topicClient.SendAsync(newMessage);
            await topicClient.CloseAsync();
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
