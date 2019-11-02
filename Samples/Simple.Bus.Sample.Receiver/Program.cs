using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Brokers.AzureServiceBus;
using Simple.Bus.Core.Brokers.AzureServiceBus.Builders;
using Simple.Bus.Core.Brokers.RabbitMQ;
using Simple.Bus.Core.Brokers.RabbitMQ.Builders;
using Simple.Bus.Core.Builders.DotNetCore;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Sample.Receiver
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
                    var logger = LoggerFactory.Create(builder => builder
                            .AddConsole()
                            .SetMinimumLevel(LogLevel.Debug))
                        .CreateLogger<Program>();

                    var azureServiceBusSection = hostContext.Configuration.GetSection("Bus:MessageContract:AzureServiceBus");
                    var connectionString = azureServiceBusSection.GetValue<string>("ConnectionString");
                    var topic = azureServiceBusSection.GetValue<string>("Topic");
                    var subscription = azureServiceBusSection.GetValue<string>("Subscription");
                    var maxConcurrentCalls = 1;
                    var autoCompleteMessage = false;
                    var handlerConfigurationAzureServiceBus = new ReceiverConfigurationAzureServiceBus(connectionString, topic, subscription, maxConcurrentCalls, autoCompleteMessage);

                    services
                        .AddBusReceiverFor<MessageContractASB>(builder => builder
                            .WithMessageHandler(new ConsumerMessage(logger).Consume)
                            .WithLogger(logger)
                            .WithAzureServiceBus(handlerConfigurationAzureServiceBus));

                    services.AddHostedService<WorkerAzureServiceBus>();

                    var rabbitMQBusSection = hostContext.Configuration.GetSection("Bus:MessageContract:RabbitMQ");
                    var hostName = rabbitMQBusSection.GetValue<string>("HostName");
                    var port = rabbitMQBusSection.GetValue<int>("Port");
                    var userName = rabbitMQBusSection.GetValue<string>("UserName");
                    var password = rabbitMQBusSection.GetValue<string>("Password");
                    var queue = rabbitMQBusSection.GetValue<string>("Queue");
                    var exchange = rabbitMQBusSection.GetValue<string>("Exchange");
                    var credentials = new CredentialsRabbitMQ(hostName, port, userName, password);

                    var handlerConfigurationRabbitMQ = new ReceiverConfigurationRabbitMQ(credentials, exchange, queue);
                    services
                        .AddBusReceiverFor<MessageContractRMQ>(builder => builder
                            .WithMessageHandler((message) =>
                            {
                                Console.WriteLine($"Receive message for rabbit mq: {message.Nome}");
                                return Task.CompletedTask;
                            })
                            .WithRabbitMQ(handlerConfigurationRabbitMQ));

                    services.AddHostedService<WorkerRabbitMQ>();
                });
    }
}
