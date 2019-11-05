using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple.Bus.Core.Brokers.AzureServiceBus;
using Simple.Bus.Core.Builders.DotNetCore;

namespace Simple.Bus.Receiver.DeadLetter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var messageContractSection = hostContext.Configuration.GetSection("Bus:MessageContract");
                    var connectionString = messageContractSection.GetValue<string>("ConnectionString");
                    var topic = messageContractSection.GetValue<string>("Topic");
                    var subscription = messageContractSection.GetValue<string>("Subscription");
                    var configuration = new ReceiverConfigurationAzureServiceBus<MessageErrorContract>(topic, subscription);
                    configuration.ReceiveOnlyDeadLetter();
                    var credentials = new CredentialsAzureServiceBus(connectionString);

                    services.AddAzureServiceBus(credentials)
                        .AddBusReceiverFor<MessageErrorContract>(builder => builder.WithConfiguration(configuration));

                    services.AddHostedService<Worker>();
                });
    }
}
