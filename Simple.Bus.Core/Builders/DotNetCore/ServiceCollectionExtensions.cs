using Microsoft.Extensions.DependencyInjection;
using System;

namespace Simple.Bus.Core.Builders.DotNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusReceiverFor<T>(this IServiceCollection services, Func<ReceiverBuilderFor<T>, ReceiverBuilderFor<T>> builder) => 
            services.AddSingleton(builder.Invoke(new ReceiverBuilderFor<T>()).Build());
        
        public static void AddBusSenderFor<T>(this IServiceCollection services, Func<SenderPipelineBuilderFor<T>, SenderPipelineBuilderFor<T>> builder) => 
            services.AddSingleton(builder.Invoke(new SenderPipelineBuilderFor<T>()).Build());
    }
}
