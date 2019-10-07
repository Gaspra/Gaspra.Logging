using Gaspra.Logging.Providers.Fluentd.Extensions;
using Gaspra.Logging.Serializer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Hosting
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureFluentd(this IHostBuilder hostBuilder, Action<HostBuilderContext, ILoggingBuilder> configureLogging)
        {
            return hostBuilder.ConfigureServices(delegate (HostBuilderContext context, IServiceCollection collection)
            {
                collection.AddLogging(delegate (ILoggingBuilder builder)
                {
                    configureLogging(context, builder);
                });
            });
        }

        public static IHostBuilder AddDefaultFluentd(this IHostBuilder hostBuilder)
        {
            hostBuilder
                .ConfigureLogging((loggingBuilder) =>
                {
                    loggingBuilder
                        .AddDefaultFluentd();
                });

            return hostBuilder;
        }

        public static ILoggingBuilder AddDefaultFluentd(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .Services
                .AddDefaultSerializer();

            loggingBuilder
                .AddFluentd();

            return loggingBuilder;
        }
    }
}
