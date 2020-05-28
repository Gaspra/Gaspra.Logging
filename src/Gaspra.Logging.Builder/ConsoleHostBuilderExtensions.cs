using Gaspra.Logging.Provider.Console;
using Gaspra.Logging.Provider.Console.Extensions;
using Microsoft.Extensions.Logging;

namespace Gaspra.Logging.Builder
{
    public static class ConsoleHostBuilderExtensions
    {
        public static ILoggingBuilder AddProviderConsole(
            this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .AddConsoleLogger();

            return loggingBuilder;
        }

        public static ILoggingBuilder AddProviderConsoleWithOptions(
            this ILoggingBuilder loggingBuilder,
            ConsoleProviderOptions consoleOptions)
        {
            loggingBuilder
                .AddConsoleLogger(consoleOptions);

            return loggingBuilder;
        }
    }
}
