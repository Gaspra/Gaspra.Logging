using Gaspra.Logging.Provider.Fluentd.Interfaces;
using Gaspra.Logging.Provider.Fluentd.Extensions;
using Gaspra.Logging.Serializer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Gaspra.Logging.Provider.Extensions;

namespace Gaspra.Logging.Provider.Fluentd
{
    public class FluentdLogger : ProviderLogger
    {
        private readonly IEnumerable<ILogSerializer> serializers;
        private readonly ProviderClient client;
        private readonly IOptions options;

        public FluentdLogger(IEnumerable<ILogSerializer> serializers, ProviderClient client, IOptions options)
        {
            this.serializers = serializers;
            this.client = client;
            this.options = options;
        }

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
                var logSerializer = serializers.GetAppropriateSerializer(logLevel, state, exception);

                if (logSerializer == null)
                {
                    ConsoleColor.Red.OutputMessage($"Couldn't derive an appropriate log serializer for: {logLevel} {formatter(state, exception)}"
                        , debug: options.Debug.On
                        , path: options.Debug.Path);
                    return;
                }

                var (serializedLog, timestamp) = logSerializer.Serialize(Name, logLevel, eventId, state, exception, formatter);

                client.Send(serializedLog, timestamp);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.OutputMessage($"Unable to log message at {DateTimeOffset.UtcNow}, due to {ex.Message}.{Environment.NewLine}{ex.StackTrace}"
                    , debug: options.Debug.On
                    , path: options.Debug.Path);
            }
        }

        public void Dispose()
        {
            client.Dispose();
        }
    }
}
