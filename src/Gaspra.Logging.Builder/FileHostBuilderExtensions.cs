using Gaspra.Logging.Provider.File;
using Gaspra.Logging.Provider.File.Extensions;
using Microsoft.Extensions.Logging;

namespace Gaspra.Logging.Builder
{
    public static class FileHostBuilderExtensions
    {
        public static ILoggingBuilder AddProviderFile(
            this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .AddFileLogger();

            return loggingBuilder;
        }

        public static ILoggingBuilder AddProviderFileWithOptions(
            this ILoggingBuilder loggingBuilder,
            FileProviderOptions fileOptions)
        {
            loggingBuilder
                .AddFileLogger(fileOptions);

            return loggingBuilder;
        }
    }
}
