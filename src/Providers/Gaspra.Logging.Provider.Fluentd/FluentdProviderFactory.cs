using Gaspra.Logging.Provider.Fluentd.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Gaspra.Logging.Provider.Fluentd
{
    public class FluentdProviderFactory : IProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<string, IProviderLogger> loggers;

        public FluentdProviderFactory(IServiceProvider serviceProvider)
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

        private IProviderLogger GetLoggerService(string name)
        {
            var logger = (IProviderLogger)serviceProvider
                .GetRequiredService(typeof(FluentdLogger));

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
