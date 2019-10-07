using Gaspra.Logging.Providers.Fluentd.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Gaspra.Logging.Providers.Fluentd
{
    public class ProviderFactory : IProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<string, IFluentdLogger> loggers;

        public ProviderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            loggers = new ConcurrentDictionary<string, IFluentdLogger>();
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

        private IFluentdLogger GetLoggerService(string name)
        {
            var logger = serviceProvider.GetService<IFluentdLogger>();

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
