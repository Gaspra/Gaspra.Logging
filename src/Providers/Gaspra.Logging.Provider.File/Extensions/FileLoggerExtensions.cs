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
            FileOptions fluentdOptions)
        {
            if (fluentdOptions == null) throw new ArgumentNullException(nameof(fluentdOptions));

            builder.Services
                .AddSingleton<IFileOptions>(fluentdOptions);

            return builder.AddFileLogger(builder.Services);
        }

        private static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder,
            IServiceCollection serviceCollection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            /*
                Try add allows us to implement these services ahead of time falling back
                to default implementations
            */

            serviceCollection
                .TryAddSingleton<IFileProviderFactory, FileProviderFactory>();

            serviceCollection
                .TryAddSingleton<IFileOptions, FileOptions>();

            serviceCollection
                .TryAddTransient<IFileLogger, FileLogger>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            builder.AddProvider(serviceProvider.GetService<IFileProviderFactory>());

            return builder;
        }
    }
}
