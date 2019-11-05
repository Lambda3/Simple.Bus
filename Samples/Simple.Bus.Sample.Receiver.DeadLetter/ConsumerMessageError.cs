using Simple.Bus.Core.Receivers;
using System;
using System.Threading.Tasks;

namespace Simple.Bus.Receiver.DeadLetter
{
    public class ConsumerMessageError : IConsumerFor<MessageErrorContract>
    {
        public Task Consume(MessageErrorContract message)
        {
            Console.WriteLine($"Error handled: {message.Nome}");
            return Task.CompletedTask;
        }
    }
}
