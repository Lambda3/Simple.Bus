# Simple Bus

A simple way to receive and send message to Azure Service Bus and RabbitMq

## Installation

You can install through [nuget](https://www.nuget.org/packages/Simple.Bus).

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

### Try out the samples

#### RabbitMQ Producer

Start a container in a container with:

````bash
docker run -d --hostname my-rabbit --name some-rabbit -p 15672:15672 rabbitmq:3.8-management
````

You can read more about the RabbitMQ image on [Docker Hub](https://hub.docker.com/_/rabbitmq).

You can access http://localhost:15672/ and log into the management page
with user `guest` and password `guest`. There, go to Exchanges and
create an exchange with name `exchange-message`, then go to Queues
and create a queue named `queue-message`, then go back to
exchanges and click on the `exchange-message` exchange, click on
bindings and add a binding to the `queue-message` queue.

On the terminal go to the directory for the `Simple.Bus.Sample.Producer.RabbitMQ`
project and run it with:

````bash
dotnet run
````

The console app will prompt you to enter a text and a message will
be created for it, create a few messages.

To see the messages go to
[the queue-message](http://localhost:15672/#/queues/%2F/queue-message),
click on the `Get messages` header, and click on the `Get Message(s)`
button and you will be able to see the messages you sent from the
terminal.

#### Azure Service Bus Producer

Start by creating the Azure Service Bus namespace, topic and subscription using
[Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli):

````bash
az group create --name sb-sample --location brazilsouth
az servicebus namespace create --name sample-ns --resource-group sb-sample --location brazilsouth --sku Standard
az servicebus topic create --name topic-message --namespace-name sample-ns --resource-group sb-sample
az servicebus topic subscription create --name simple-bus-consumer --topic-name topic-message --namespace-name sample-ns --resource-group sb-sample
````

Get the connection string with:

````bash
az servicebus namespace authorization-rule keys list --namespace-name sample-ns --resource-group sb-sample --name RootManageSharedAccessKey --query primaryConnectionString -o tsv
````

Update the connection string on `Program.cs` in the
`Simple.Bus.Sample.Producer.AzureServiceBus` project.

Use the `Simple.Bus.Sample.Receiver` project to read the message.
Or you could use the Service Bus Explorer from Azure Portal to
peek at the messages, or the
[Service Bus Explorer application](https://github.com/paolosalvatori/ServiceBusExplorer).

#### Receiver (Azure Service Bus and RabbitMQ)

Look at how to configure the Azure Service Bus on the previous topic.

Update the `appsettings.Development.json` file to include the connection
string.

Run the application with:

````bash
cd Samples/Simple.Bus.Sample.Receiver
dotnet run
````

Or you can build and run the container with the following command. Run it from
the project root directory (where the .sln file is):

````bash
docker build -t receiver -f .\Samples\Simple.Bus.Sample.Receiver\Dockerfile .
docker run --rm -ti receiver bash
````

To run the dead letter sample use the same process, but substitute for
the `Simple.Bus.Sample.Receiver.DeadLetter` directory.

## Extensions

You can extend Serialization, Cryptography, Pipeline and Logging.

## Contributing

The main supported IDE for development is Visual Studio 2019.

Questions, comments, bug reports, and pull requests are all welcome.
Bug reports that include steps to reproduce (including code) are
preferred. Even better, make them in the form of pull requests.
Before you start to work on an existing issue, check if it is not assigned
to anyone yet, and if it is, talk to that person.

## License

This software is open source, licensed under the MIT License.
See [LICENSE](https://github.com/Lambda3/Simple.Bus/blob/master/LICENSE.txt) for details.
Check out the terms of the license before you contribute, fork, copy or do anything
with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project and agree to
mention clearly if do not agree to these terms. Your work will be licensed with the project at MIT, along the rest of the code.
