﻿using Microsoft.Extensions.Logging;
using Simple.Bus.Core.Receivers;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Builders.DotNetCore
{
    public class ConsumerDefault<T> : IConsumerFor<T>
    {
        private readonly ILogger logger;

        public ConsumerDefault(ILogger logger)
        {
            this.logger = logger;
        }

        public Task Consume(T message)
        {
            logger.LogInformation($"Message received. Please, implement an interface IConsumerFor");
            return Task.CompletedTask;
        }
    }
}
