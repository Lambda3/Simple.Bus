using Microsoft.Extensions.DependencyInjection;
using Simple.Bus.Core.Brokers.RabbitMQ;
using Simple.Bus.Core.Brokers.RabbitMQ.Builders;
using Simple.Bus.Core.Builders;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Senders.Pipelines;
using Simple.Bus.Core.Senders;

namespace Simple.Bus.Sample.Producer.RabbitMQ
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
            var hostName = "localhost";
            var port = 5672;
            var userName = "guest";
            var password = "guest";
            var credentials = new CredentialsRabbitMQ(hostName, port, userName, password);
            var exchange = "exchange-message";

            var sender = new SenderPipelineBuilderFor<MessageForProducer>()
                .WithRabbitMq(credentials, exchange)
                .WithLogger(loggerPipeline)
                .WithLogger(loggerSender).Build();

            do
            {
                Console.WriteLine("Enter message for rabbitMQ (or quit to exit)");
                Console.WriteLine("Enter message (or quit to exit)");
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
