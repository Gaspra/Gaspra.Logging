using Gaspra.Logging.Provider.File.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Provider.File.Extensions
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder)
        {
            return builder.AddFileLogger(builder.Services);
        }

        public static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder,
            Options fluentdOptions)
        {
            if (fluentdOptions == null) throw new ArgumentNullException(nameof(fluentdOptions));

            builder.Services
                .AddSingleton<IOptions>(fluentdOptions);

            return builder.AddFileLogger(builder.Services);
        }

        private static ILoggingBuilder AddFileLogger(
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

            //serviceCollection
            //    .AddServiceIfNotImplemented<ProviderFactory>(typeof(FileProviderFactory), ServiceLifetime.Singleton)
            //    .AddServiceIfNotImplemented<IOptions>(typeof(Options), ServiceLifetime.Singleton)
            //    .AddServiceIfNotImplemented<ProviderLogger>(typeof(FileLogger), ServiceLifetime.Transient);

            //serviceCollection
            //    .TryAddSingleton<ProviderClient, Client>();

            //serviceCollection
            //    .TryAddTransient<ProviderPacker, LogPacker>();

            //serviceCollection
            //    .TryAddTransient<IClientTimer, ClientTimer>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            builder.AddProvider(serviceProvider.GetService<ProviderFactory>());

            return builder;
        }
    }
}
