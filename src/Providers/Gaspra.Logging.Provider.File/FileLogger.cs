using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Provider.File
{
    public class FileLogger : ProviderLogger
    {
        public string Name { get; set; }

        /*
            Refrain from using this, to supress log events use the
            `.AddConfiguration(configuration.GetSection("Logging"))`
            built in log supression set up when configuring the logging
        */
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

            /*
                Determine the appropriate serializer to use for this log event,
                GetAppropriateSerializer will return the FirstOrDefault serializer
                that determines this event as IsSerializiable.

                Once determined the event is serialized to a Dictionary<string, object>
                and sent with a timestamp.
            */
            try
            {
                System.Console.WriteLine($"{nameof(FileLogger)} - {Name} - {logLevel} - {formatter(state, exception)}");
            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
            //client.Dispose();
        }
    }
}
