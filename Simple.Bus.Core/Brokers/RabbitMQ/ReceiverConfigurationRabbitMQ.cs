using RabbitMQ.Client;
using Simple.Bus.Core.Receivers;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public class ReceiverConfigurationRabbitMQ : ReceiverConfiguration
    {
        public CredentialsRabbitMQ Credentials { get; }
        public QueueConfigurationRabbitMQ Queue { get; }
        public string Exchange { get; }
        public string TypeForExchange { get; }

        private const bool AutoCompleteDefault = false;

        public ReceiverConfigurationRabbitMQ(CredentialsRabbitMQ credentials, string exchange, string queue)
            : this(credentials, exchange, ExchangeType.Fanout, AutoCompleteDefault, new QueueConfigurationRabbitMQ(queue))
        {
        }

        public ReceiverConfigurationRabbitMQ(CredentialsRabbitMQ credentials, string exchange, string exchangeType, bool autoComplete, QueueConfigurationRabbitMQ queue)
            : base(autoComplete)
        {

            Credentials = credentials;
            Exchange = exchange;
            TypeForExchange = exchangeType;
            Queue = queue;
        }
    }

    public class QueueConfigurationRabbitMQ
    {
        public string Name { get; set; }
        public bool Durable { get; set; } = true;
        public bool Exclusive { get; set; } = false;
        public bool AutoDelete { get; set; } = false;

        public QueueConfigurationRabbitMQ(string name)
        {
            Name = name;
        }
    }
}
