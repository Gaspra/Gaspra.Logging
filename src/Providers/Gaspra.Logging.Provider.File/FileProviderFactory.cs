using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gaspra.Logging.Provider.File
{
    public class FileProviderFactory : ProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<string, ProviderLogger> loggers;

        public FileProviderFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            loggers = new ConcurrentDictionary<string, ProviderLogger>();
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

        private ProviderLogger GetLoggerService(string name)
        {
            var loggers = serviceProvider.GetServices<ProviderLogger>();

            var logger = loggers
                .Where(l => l
                    .GetType()
                    .Equals(typeof(FileLogger)))
                .FirstOrDefault();

            if (logger == null)
            {
                throw new NullReferenceException($"Unable to get logger with type {typeof(FileLogger)}, ensure provider is registered.");
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
