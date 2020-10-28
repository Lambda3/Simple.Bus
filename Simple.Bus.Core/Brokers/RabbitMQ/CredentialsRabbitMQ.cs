using RabbitMQ.Client;

namespace Simple.Bus.Core.Brokers.RabbitMQ
{
    public class CredentialsRabbitMQ : ICredentials<ConnectionFactory>
    {
        private readonly ConnectionFactory factory;

        public CredentialsRabbitMQ(string hostName, int port, string userName, string password)
        {
            factory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
                DispatchConsumersAsync = true
            };
        }
        public ConnectionFactory Get() => factory;
    }
}
