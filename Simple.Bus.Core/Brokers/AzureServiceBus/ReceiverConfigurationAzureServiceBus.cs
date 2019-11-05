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

        private const int MaxConcorrentCallDefault = 1;
        private const bool AutoCompleteDefault = false;

        public ReceiverConfigurationAzureServiceBus(string topicName, string subscriptionName, int maxConcorrentCalls, bool autoCompleteMessage)
            : base(autoCompleteMessage)
        {
            TopicName = topicName;
            this.subscriptionName = subscriptionName;
            MaxConcorrentCalls = maxConcorrentCalls;
            IsReceiveOnlyDeadLetter = false;
        }

        public ReceiverConfigurationAzureServiceBus(string topicName, string subscriptionName)
            : this(topicName, subscriptionName, MaxConcorrentCallDefault, AutoCompleteDefault)
        {
        }

        public string SubscriptionName { get => IsReceiveOnlyDeadLetter ? $"{subscriptionName}{DeadLetterSufix}" : subscriptionName; }

        public void ReceiveOnlyDeadLetter()
        {
            IsReceiveOnlyDeadLetter = true;
        }
    }
}
