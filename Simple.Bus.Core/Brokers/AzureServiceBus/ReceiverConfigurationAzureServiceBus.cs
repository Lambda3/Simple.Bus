using Simple.Bus.Core.Receivers;

namespace Simple.Bus.Core.Brokers.AzureServiceBus
{
    public class ReceiverConfigurationAzureServiceBus<T> : ReceiverConfiguration<T>
    {
        const string DeadLetterSufix = "/$DeadLetterQueue";
        private readonly string subscriptionName;
        public string TopicName { get; }
        public bool IsReceiveOnlyDeadLetter { get; private set; }
        public int MaxConcorrentCalls { get; set; }
        public int CustomRetryCount { get; set; }
        public int CustomRetryDelay { get; set; }

        private const int MaxConcorrentCallDefault = 1;
        private const bool AutoCompleteDefault = false;
        private const int CustomRetryCountDefault = 3;
        private const int CustomRetryDelayDefault = 10;

        public ReceiverConfigurationAzureServiceBus(string topicName, string subscriptionName, int maxConcorrentCalls, bool autoCompleteMessage, int customRetryCount, int customRetryDelay)
            : base(autoCompleteMessage)
        {
            TopicName = topicName;
            this.subscriptionName = subscriptionName;
            MaxConcorrentCalls = maxConcorrentCalls;
            IsReceiveOnlyDeadLetter = false;
            CustomRetryCount = customRetryCount;
            CustomRetryDelay = customRetryDelay;
        }

        public ReceiverConfigurationAzureServiceBus(string topicName, string subscriptionName, int maxConcorrentCalls, bool autoCompleteMessage)
            : this(topicName, subscriptionName, maxConcorrentCalls, autoCompleteMessage, CustomRetryCountDefault, CustomRetryDelayDefault)
        {
        }


        public ReceiverConfigurationAzureServiceBus(string topicName, string subscriptionName)
            : this(topicName, subscriptionName, MaxConcorrentCallDefault, AutoCompleteDefault, CustomRetryCountDefault, CustomRetryDelayDefault)
        {
        }

        public string SubscriptionName { get => IsReceiveOnlyDeadLetter ? $"{subscriptionName}{DeadLetterSufix}" : subscriptionName; }

        public void ReceiveOnlyDeadLetter()
        {
            IsReceiveOnlyDeadLetter = true;
        }
    }
}
