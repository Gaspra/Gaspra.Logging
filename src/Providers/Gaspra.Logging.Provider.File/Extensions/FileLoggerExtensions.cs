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
            FileProviderOptions fileOptions)
        {
            if (fileOptions == null) throw new ArgumentNullException(nameof(fileOptions));

            builder.Services
                .AddSingleton<IFileProviderOptions>(fileOptions);

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
                .TryAddSingleton<IFileProviderOptions, FileProviderOptions>();

            serviceCollection
                .TryAddSingleton<IFileClientTimer, FileClientTimer>();

            serviceCollection
                .TryAddSingleton<IFileClient, FileClient>();

            serviceCollection
                .TryAddTransient<IFileLogger, FileLogger>();

            serviceCollection
                .TryAddSingleton<IFilePacker, FilePacker>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            builder.AddProvider(serviceProvider.GetService<IFileProviderFactory>());

            return builder;
        }
    }
}
