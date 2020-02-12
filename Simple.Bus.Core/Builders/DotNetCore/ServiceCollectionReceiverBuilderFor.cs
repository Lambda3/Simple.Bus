using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Simple.Bus.Core.Brokers.AzureServiceBus;
using Simple.Bus.Core.Brokers.RabbitMQ;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Receivers;
using Simple.Bus.Core.Receivers.Pipelines;
using Simple.Bus.Core.Serializers;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Builders.DotNetCore
{
    public class ServiceCollectionReceiverBuilderFor<T> where T : class
    {
        private readonly IServiceCollection services;

        public ServiceCollectionReceiverBuilderFor(IServiceCollection services)
        {
            this.services = services;
        }

        public ServiceCollectionReceiverBuilderFor<T> WithMessageHandler(Func<T, Task> handlerMessageFunction)
        {
            services.AddTransient<IConsumerFor<T>>(x => new LambdaConsumer<T>(handlerMessageFunction));
            return this;
        }

        public ServiceCollectionReceiverBuilderFor<T> WithMessageHandler<Q>() where Q : class, IConsumerFor<T>
        {
            services.AddTransient<IConsumerFor<T>, Q>();
            return this;
        }

        public ServiceCollectionReceiverBuilderFor<T> WithPipelineReceiver<Q>() where Q : class, IPipelineReceiverFor<T>
        {
            services.AddTransient<IPipelineReceiverFor<T>, Q>();
            return this;
        }

        public ServiceCollectionReceiverBuilderFor<T> WithReceiver<Q>(ReceiverConfiguration<T> configuration) where Q : class, IReceiverFor<T>
        {
            services.AddSingleton(configuration.GetType(), configuration);
            services.AddSingleton<IReceiverFor<T>, Q>();
            return this;
        }

        public ServiceCollectionReceiverBuilderFor<T> WithConfiguration(ReceiverConfigurationAzureServiceBus<T> configuration)
        {
            WithReceiver<ReceiverAzureServiceBusFor<T>>(configuration);
            return this;
        }

        public ServiceCollectionReceiverBuilderFor<T> WithConfiguration(ReceiverConfigurationRabbitMQ<T> configuration)
        {
            WithReceiver<ReceiverRabbitMQFor<T>>(configuration);
            return this;
        }

        public IServiceCollection Build()
        {
            services.TryAddTransient<T>();
            services.TryAddSingleton<ISerializer, SerializerDefault>();
            services.TryAddSingleton<ICryptography, CryptographyDefault>();
            services.TryAddTransient<IPipelineReceiverFor<T>, PipelineReceiverFor<T>>();
            services.TryAddTransient<IConsumerFor<T>, ConsumerDefault<T>>();
            services.AddTransient<Func<IPipelineReceiverFor<T>>>(x => () => x.GetService<IPipelineReceiverFor<T>>());
            services.AddSingleton<ResourcesRabbitMQ>();

            return services;
        }
    }
}
