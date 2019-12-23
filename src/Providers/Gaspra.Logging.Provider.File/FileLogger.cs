using Gaspra.Logging.Provider.File.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Provider.File
{
    public class FileLogger : IFileLogger
    {
        public string Name { get; set; }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || formatter == null)
            {
                return;
            }

            try
            {
                Console.WriteLine($"{nameof(FileLogger)} - {Name} - {logLevel} - {formatter(state, exception)}");
            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {

        }
    }
}
