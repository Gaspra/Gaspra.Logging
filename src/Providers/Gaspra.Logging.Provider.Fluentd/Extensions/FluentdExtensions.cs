using Gaspra.Logging.Provider.Fluentd.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider.Fluentd.Extensions
{
    public static class FluentdExtensions
    {
        public static ILoggingBuilder AddFluentd(
            this ILoggingBuilder builder)
        {
            return builder.AddFluentd(builder.Services);
        }

        public static ILoggingBuilder AddFluentd(
            this ILoggingBuilder builder,
            Options fluentdOptions)
        {
            if (fluentdOptions == null) throw new ArgumentNullException(nameof(fluentdOptions));

            builder.Services
                .AddSingleton<IOptions>(fluentdOptions);

            return builder.AddFluentd(builder.Services);
        }

        private static ILoggingBuilder AddFluentd(
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
                .TryAddSingleton<ProviderFactory, FluentdProviderFactory>();

            serviceCollection
                .TryAddSingleton<IOptions, Options>();

            serviceCollection
                .TryAddSingleton<ProviderClient, Client>();

            serviceCollection
                .TryAddTransient<ProviderLogger, FluentdLogger>();

            serviceCollection
                .TryAddTransient<ProviderPacker, LogPacker>();

            serviceCollection
                .TryAddTransient<IClientTimer, ClientTimer>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            builder.AddProvider(serviceProvider.GetService<ProviderFactory>());

            return builder;
        }
    }
}
