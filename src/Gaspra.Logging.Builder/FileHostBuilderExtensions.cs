using Gaspra.Logging.Provider.File.Extensions;
using Microsoft.Extensions.Logging;

namespace Gaspra.Logging.Builder
{
    public static class FileHostBuilderExtensions
    {
        public static ILoggingBuilder AddProviderFluentd(
            this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .AddFileLogger();

            return loggingBuilder;
        }
    }
}
