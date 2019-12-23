using Gaspra.Logging.Provider.Fluentd.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider.Fluentd.Extensions
{
    public static class FluentdLoggerExtensions
    {
        public static ILoggingBuilder AddFluentdLogger(
            this ILoggingBuilder builder)
        {
            return builder.AddFluentdLogger(builder.Services);
        }

        public static ILoggingBuilder AddFluentdLogger(
            this ILoggingBuilder builder,
            FluentdOptions fluentdOptions)
        {
            if (fluentdOptions == null) throw new ArgumentNullException(nameof(fluentdOptions));

            builder.Services
                .AddSingleton<IFluentdOptions>(fluentdOptions);

            return builder.AddFluentdLogger(builder.Services);
        }

        private static ILoggingBuilder AddFluentdLogger(
            this ILoggingBuilder builder,
            IServiceCollection serviceCollection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            /*
                Using the try add extension methods, add services
                for the Fluentd logging. This allows for services
                to be added earlier and not overwritten here (like
                overriding the options).
            */

            serviceCollection
                .TryAddSingleton<IFluentdProviderFactory, FluentdProviderFactory>();

            serviceCollection
                .TryAddSingleton<IFluentdOptions, FluentdOptions>();

            serviceCollection
                .TryAddSingleton<IFluentdClient, FluentdClient>();

            serviceCollection
                .TryAddTransient<IFluentdLogger, FluentdLogger>();

            serviceCollection
                .TryAddTransient<IFluentdPacker, FluentdPacker>();

            serviceCollection
                .TryAddTransient<IFluentdClientTimer, FluentdClientTimer>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            builder.AddProvider(serviceProvider.GetService<IFluentdProviderFactory>());

            return builder;
        }
    }
}
