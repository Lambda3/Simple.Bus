using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Simple.Bus.Core.Brokers.AzureServiceBus;
using Simple.Bus.Core.Builders.DotNetCore;
using Simple.Bus.Receiver.DeadLetter;
using System;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var messageContractSection = hostContext.Configuration.GetSection("Bus:MessageContract");
        var connectionString = messageContractSection.GetValue<string>("ConnectionString");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception($"Service Bus {nameof(connectionString)} cannot be null.");
        var topic = messageContractSection.GetValue<string>("Topic");
        if (string.IsNullOrWhiteSpace(topic))
            throw new Exception($"Service Bus {nameof(topic)} cannot be null.");
        var subscription = messageContractSection.GetValue<string>("Subscription");
        if (string.IsNullOrWhiteSpace(subscription))
            throw new Exception($"Service Bus {nameof(subscription)} cannot be null.");
        var configuration = new ReceiverConfigurationAzureServiceBus<MessageErrorContract>(topic, subscription);
        configuration.ReceiveOnlyDeadLetter();
        var credentials = new CredentialsAzureServiceBus(connectionString);
        services.AddAzureServiceBus(credentials)
            .AddBusReceiverFor<MessageErrorContract>(builder => builder.WithConfiguration(configuration));
        services.AddHostedService<Worker>();
    }).Build().Run();
