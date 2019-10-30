using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Simple.Bus.Core.Loggers
{
    public class LoggerFactory
    {
        public static ILogger CreateLogger<T>()
        {
            return new ServiceCollection()
               .AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Information))
               .BuildServiceProvider()
               .GetService<ILoggerFactory>().CreateLogger<T>();
        }
    }
}
