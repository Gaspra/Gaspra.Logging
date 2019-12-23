using Gaspra.Logging.Provider.Console.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleProviderFactory : IConsoleProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<string, IProviderLogger> loggers;

        public ConsoleProviderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            loggers = new ConcurrentDictionary<string, IProviderLogger>();
        }

        /*
            Creates a new logger, this gets called when ILogger<Class> is
            injected into the constructor creating a logger with the
            fullname of the class `Class`
        */
        public ILogger CreateLogger(string name)
        {
            var logger = loggers.GetOrAdd(name, GetLoggerService(name));

            return logger;
        }

        private IConsoleLogger GetLoggerService(string name)
        {
            var logger = serviceProvider.GetService<IConsoleLogger>();

            if(logger == null)
            {
                throw new NullReferenceException(
                    $"Unable to get logger with type {nameof(IConsoleLogger)}, ensure provider is registered.");
            }

            logger.Name = name;

            return logger;
        }

        public void Dispose()
        {
            foreach (var logger in loggers)
            {
                logger
                    .Value
                    .Dispose();
            }
        }
    }
}
