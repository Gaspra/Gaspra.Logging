using Gaspra.Logging.Provider.Console.Extensions;
using Gaspra.Logging.Provider.Console.Interfaces;
using Gaspra.Logging.Provider.Extensions;
using Gaspra.Logging.Serializer;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleLogger : IConsoleLogger
    {
        private readonly IConsoleOptions options;
        private readonly IEnumerable<ILogSerializer> serializers;

        public ConsoleLogger(IConsoleOptions options, IEnumerable<ILogSerializer> serializers)
        {
            this.options = options;
            this.serializers = serializers
                .Where(s => options
                    .AppropriateSerializers
                    .Contains(s.GetType()));
        }

        public string Name { get; set; }

        /*

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

            */
            try
            {
                var logSerializer = serializers.GetAppropriateSerializer(logLevel, state, exception);

                var (serializedLog, timestamp) = logSerializer.Serialize(Name, logLevel, eventId, state, exception, formatter);

                var (back, fore) = logLevel.ConsoleColour();

                options.ConsoleFormat
                    .Replace("timestamp", timestamp.ToString("HH:mm:ss.fff"))
                    .Replace("level", logLevel.ShortString())
                    .Replace("name", Name)
                    .OutputMessage(back, fore);

                string.Join(serializedLog.Values.ToString(), ", ")
                    .OutputMessage(lineEnding: true);
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
