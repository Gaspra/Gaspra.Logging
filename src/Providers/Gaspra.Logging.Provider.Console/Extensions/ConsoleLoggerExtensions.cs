using Gaspra.Logging.Provider.Console.Interfaces;
using Gaspra.Logging.Provider.Console.Serializer;
using Gaspra.Logging.Serializer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider.Console.Extensions
{
    public static class ConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddConsoleLogger(
            this ILoggingBuilder builder)
        {
            return builder.AddConsoleLogger(builder.Services);
        }

        public static ILoggingBuilder AddConsoleLogger(
            this ILoggingBuilder builder,
            ConsoleProviderOptions consoleOptions)
        {
            if (consoleOptions == null) throw new ArgumentNullException(nameof(consoleOptions));

            builder.Services
                .AddSingleton<IConsoleProviderOptions>(consoleOptions);

            return builder.AddConsoleLogger(builder.Services);
        }

        private static ILoggingBuilder AddConsoleLogger(
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
                .TryAddSerializer(typeof(ConsoleSerializer));

            serviceCollection
                .TryAddSingleton<IConsoleProviderFactory, ConsoleProviderFactory>();

            serviceCollection
                .TryAddSingleton<IConsoleProviderOptions, ConsoleProviderOptions>();

            serviceCollection
                .TryAddTransient<IConsoleLogger, ConsoleLogger>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            builder.AddProvider(serviceProvider.GetService<IConsoleProviderFactory>());

            return builder;
        }
    }
}
