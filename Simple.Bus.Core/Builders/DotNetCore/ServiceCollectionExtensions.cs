using Microsoft.Extensions.DependencyInjection;
using Simple.Bus.Core.Brokers.AzureServiceBus;
using Simple.Bus.Core.Brokers.RabbitMQ;
using Simple.Bus.Core.Cryptographers;
using Simple.Bus.Core.Serializers;
using System;

namespace Simple.Bus.Core.Builders.DotNetCore
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureServiceBus(this IServiceCollection services, CredentialsAzureServiceBus credentials)
        {
            services.AddSingleton(credentials);
            return services;
        }

        public static IServiceCollection AddRabbitMQ(this IServiceCollection services,
            CredentialsRabbitMQ credentials)
        {
            services.AddSingleton(credentials);
            return services;
        }

        public static IServiceCollection AddBusReceiverFor<T>(this IServiceCollection services, Func<ServiceCollectionReceiverBuilderFor<T>, ServiceCollectionReceiverBuilderFor<T>> builder) where T : class
        {
            builder(new ServiceCollectionReceiverBuilderFor<T>(services)).Build();

            return services;
        }

        public static IServiceCollection WithSerializer<T>(this IServiceCollection services) where T : class, ISerializer
        {
            services.AddSingleton<T>();
            return services;
        }

        public static IServiceCollection WithCryptography<T>(this IServiceCollection services) where T : class, ICryptography
        {
            services.AddSingleton<T>();
            return services;
        }
    }
}
