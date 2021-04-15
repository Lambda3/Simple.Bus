using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using Simple.Bus.Core.Brokers.AzureServiceBus;
using Simple.Bus.Core.Brokers.RabbitMQ;
using Simple.Bus.Core.Builders.DotNetCore;
using System;
using Simple.Bus.Sample.Receiver;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var azureServiceBusSection = hostContext.Configuration.GetSection("Bus:MessageContract:AzureServiceBus");

        var connectionString = azureServiceBusSection.GetValue<string>("ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception($"Service Bus {nameof(connectionString)} cannot be null.");
        var topic = azureServiceBusSection.GetValue<string>("Topic");
        if (string.IsNullOrWhiteSpace(topic))
            throw new Exception($"Service Bus {nameof(topic)} cannot be null.");
        var subscription = azureServiceBusSection.GetValue<string>("Subscription");
        if (string.IsNullOrWhiteSpace(subscription))
            throw new Exception($"Service Bus {nameof(subscription)} cannot be null.");
        var maxConcurrentCalls = 1;
        var autoCompleteMessage = false;
        var configurationAzureServiceBus = new ReceiverConfigurationAzureServiceBus<MessageContractASB>(topic, subscription, maxConcurrentCalls, autoCompleteMessage);
        var credentialsAzureServiceBus = new CredentialsAzureServiceBus(connectionString);

        services
            .AddAzureServiceBus(credentialsAzureServiceBus)
            .AddBusReceiverFor<MessageContractASB>(builder => builder
                .WithConfiguration(configurationAzureServiceBus)
                .WithMessageHandler<ConsumerMessage>());

        services.AddHostedService<WorkerAzureServiceBus>();

        var rabbitMQBusSection = hostContext.Configuration.GetSection("Bus:MessageContract:RabbitMQ");

        var hostName = rabbitMQBusSection.GetValue<string>("HostName");
        if (string.IsNullOrWhiteSpace(hostName))
            throw new Exception($"Rabbit MQ {nameof(hostName)} cannot be null.");
        var port = rabbitMQBusSection.GetValue<ushort>("Port");
        if (port == 0)
            throw new Exception($"Rabbit MQ {nameof(port)} cannot be null.");
        var userName = rabbitMQBusSection.GetValue<string>("UserName");
        if (string.IsNullOrWhiteSpace(userName))
            throw new Exception($"Rabbit MQ {nameof(userName)} cannot be null.");
        var password = rabbitMQBusSection.GetValue<string>("Password");
        if (string.IsNullOrWhiteSpace(password))
            throw new Exception($"Rabbit MQ {nameof(password)} cannot be null.");
        var queue = rabbitMQBusSection.GetValue<string>("Queue");
        if (string.IsNullOrWhiteSpace(queue))
            throw new Exception($"Rabbit MQ {nameof(queue)} cannot be null.");
        var exchange = rabbitMQBusSection.GetValue<string>("Exchange");
        if (string.IsNullOrWhiteSpace(exchange))
            throw new Exception($"Rabbit MQ {nameof(exchange)} cannot be null.");
        var credentialsRabbitMQ = new CredentialsRabbitMQ(hostName, port, userName, password);

        var configurationRabbitMQ = new ReceiverConfigurationRabbitMQ<MessageContractRMQ>(exchange, ExchangeType.Direct, false, new QueueConfigurationRabbitMQ(queue) { Durable = true });
        services
            .AddRabbitMQ(credentialsRabbitMQ)
            .AddBusReceiverFor<MessageContractRMQ>(builder => builder
                .WithConfiguration(configurationRabbitMQ)
                .WithMessageHandler<ConsumerMessageRMQ>());

        services.AddHostedService<WorkerRabbitMQ>();
    }).Build().Run();
