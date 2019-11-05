using System;
using System.Threading.Tasks;

namespace Simple.Bus.Core.Receivers
{
    public class LambdaConsumer<T> : IConsumerFor<T>
    {
        private readonly Func<T, Task> handlerMessageFunction;

        public LambdaConsumer(Func<T, Task> handlerMessageFunction) => this.handlerMessageFunction = handlerMessageFunction ?? throw new ArgumentNullException(nameof(handlerMessageFunction));

        public Task Consume(T message) => handlerMessageFunction(message);
    }

}
