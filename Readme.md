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
		.UseMessageHandler((message) => 
		{
			Console.WriteLine($"Message received {message}");
			return Task.Completed;
		})
		.UseLogger(logger)
		.UseAzureServiceBus(receiverConfigurationAzureServiceBus));
```
- RabbitMQ

You should use the class ReceiverConfigurationRabbitMQ to configure rabbitMQ.

```c#
services
	.AddBusReceiverFor<YourMessage>(builder => builder
   		.UseMessageHandler((message) =>
   		{
   			Console.WriteLine($"Receive message for rabbit mq: {message.Nome}");
   			return Task.CompletedTask;
   		})
   		.UseRabbitMQ(handlerConfigurationRabbitMQ));
```

After that, you can inject ```ReceiverBuilderFor<T>```

### Send


- RabbitMQ
			
```c#
var sender = new SenderPipelineBuilderFor<YourMessage>().UseRabbitMq(credentials, exchange).Build();

```

- Azure Service Bus

```c#
var sender = new SenderPipelineBuilderFor<YourMessage>().UseAzureServiceBus(connectionString, topicName).Build();

```

or
 
```c#
services.AddBusSenderFor<YourMessage>(builder => builder.UseAzureServiceBus(connectionString, topicName));

```

## Samples

There are four samples:
- Samples/Simple.Bus.Sample.Producer.AzureServiceBus
- Samples/Simple.Bus.Sample.Producer.RabbitMQ
- Samples/Simple.Bus.Sample.Receiver
- Samples/Simple.Bus.ReceiverDeadLetter

## Extensions

You can extend Serialization, Cryptography, Pipeline and Logging.
