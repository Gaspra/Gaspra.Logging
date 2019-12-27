using Gaspra.Logging.Provider.Extensions;
using Gaspra.Logging.Provider.File.Interfaces;
using Gaspra.Logging.Serializer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Provider.File
{
    public class FileLogger : IFileLogger
    {
        private readonly IFileProviderOptions options;
        private readonly IEnumerable<ILogSerializer> serializers;
        private readonly IFileClient client;

        public FileLogger(IFileProviderOptions options, IEnumerable<ILogSerializer> serializers, IFileClient client)
        {
            this.options = options;
            this.serializers = serializers;
            this.client = client;
        }


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
                var logSerializer = serializers.GetAppropriateSerializer(logLevel, state, exception);

                if (logSerializer == null)
                {
                    //todo
                    throw new NotImplementedException("Unable to find an appropriate serializer for the log event");
                }

                var (serializedLog, timestamp) = logSerializer.Serialize(Name, logLevel, eventId, state, exception, formatter);

                client.Send(serializedLog, timestamp);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.OutputMessage($"Unable to log message at {DateTimeOffset.UtcNow}, due to {ex.Message}.{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public void Dispose()
        {

        }
    }
}
