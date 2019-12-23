using Gaspra.Logging.Provider.File.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gaspra.Logging.Provider.File
{
    public class FileProviderFactory : IFileProviderFactory
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ConcurrentDictionary<string, IProviderLogger> loggers;

        public FileProviderFactory(IServiceProvider serviceProvider)
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

        private IFileLogger GetLoggerService(string name)
        {
            var logger = serviceProvider.GetService<IFileLogger>();

            if (logger == null)
            {
                throw new NullReferenceException(
                    $"Unable to get logger with type {nameof(IFileLogger)}, ensure provider is registered.");
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
