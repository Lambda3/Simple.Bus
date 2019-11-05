namespace Simple.Bus.Core.Brokers.AzureServiceBus
{
    public class CredentialsAzureServiceBus: ICredentials<string>
    {
        public CredentialsAzureServiceBus(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private readonly string connectionString;

        public string Get() => connectionString;
    }
}
