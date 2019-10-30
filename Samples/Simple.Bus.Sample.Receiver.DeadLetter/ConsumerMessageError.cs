using System;
using System.Threading.Tasks;

namespace Simple.Bus.Receiver.DeadLetter
{
    public class ConsumerMessageError 
    {
        public Task Consume(MessageErrorContract message)
        {
            Console.WriteLine($"Error handled: {message.Nome}");
            return Task.CompletedTask;
        }
    }
}
