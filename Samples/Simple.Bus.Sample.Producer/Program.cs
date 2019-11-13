using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Brokers.AzureServiceBus.Builders;
using Simple.Bus.Core.Builders;
using Simple.Bus.Core.Senders;
using Simple.Bus.Core.Senders.Pipelines;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Sample.Producer.AzureServiceBus
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            var serviceProvider = new ServiceCollection()
                 .AddLogging(c => c.AddConsole().AddDebug())
                 .BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var loggerPipeline = loggerFactory.CreateLogger<ISenderPipelineFor<MessageForProducer>>();
            var loggerSender = loggerFactory.CreateLogger<ISenderFor<MessageForProducer>>();

            var connectionString = "";
            var topicName = "topic-message";

            var sender = new SenderPipelineBuilderFor<MessageForProducer>()
                .WithLogger(loggerSender)
                .WithLogger(loggerPipeline)
                .WithAzureServiceBus(connectionString, topicName).Build();

            do
            {
                Console.WriteLine("Enter message for azure service bus (or quit to exit)");
                Console.Write("> ");
                string value = Console.ReadLine();

                if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                    break;

                await sender.SendAsync(new MessageForProducer { Nome = value });
            }

            while (true);
        }
    }
}
