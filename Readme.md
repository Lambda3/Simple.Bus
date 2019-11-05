# Simple Bus

A simple way to receive and send message to Azure Service Bus and RabbitMq

## Installation

You can install through [nuget](https://www.nuget.org/packages/Simple.Bus) 

## Configuration
### Receive

- Azure Service Bus 

You should use the class ReceiverConfigurationAzureServiceBus to configure service bus.
```c#
services
	.AddBusReceiverFor<YourMessage>(builder => builder
		.WithMessageHandler(message => 
		{
			Console.WriteLine($"Message received {message}");
			return Task.Completed;
		})
		.WithLogger(logger)
		.WithAzureServiceBus(receiverConfigurationAzureServiceBus));
```
- RabbitMQ

You should use the class ReceiverConfigurationRabbitMQ to configure rabbitMQ.
```c#
services
	.AddBusReceiverFor<YourMessage>(builder => builder
   		.WithMessageHandler(message =>
   		{
   			Console.WriteLine($"Receive message for rabbit mq: {message.Nome}");
   			return Task.CompletedTask;
   		})
   		.WithRabbitMQ(handlerConfigurationRabbitMQ));
```

After that, you can inject ```ReceiverBuilderFor<YourMessage>```

You must call Start for ```IReceiverFor<YourMessage>``` to start listening

You must call Stop for ```IReceiverFor<YourMessage>``` to stop listening

### Send


- RabbitMQ
			
```c#
var sender = new SenderPipelineBuilderFor<YourMessage>().WithRabbitMq(credentials, exchange).Build();

```

- Azure Service Bus

```c#
var sender = new SenderPipelineBuilderFor<YourMessage>().WithAzureServiceBus(connectionString, topicName).Build();

```

or
 
```c#
services.AddBusSenderFor<YourMessage>(builder => builder.WithAzureServiceBus(connectionString, topicName));

```

## Samples

There are four samples:
- Samples/Simple.Bus.Sample.Producer.AzureServiceBus
- Samples/Simple.Bus.Sample.Producer.RabbitMQ
- Samples/Simple.Bus.Sample.Receiver
- Samples/Simple.Bus.ReceiverDeadLetter

## Extensions

You can extend Serialization, Cryptography, Pipeline and Logging.
