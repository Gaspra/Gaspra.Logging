using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Serializer
{
    public class DefaultSerializer : ILogSerializer
    {
        private JsonSerializerSettings SerializerSettings { get; }
        private IDictionary<string, string> Properties { get; set; } = null;

        public DefaultSerializer(ILogProperties logProperties)
        {
            if (logProperties != null)
            {
                Properties = logProperties.Properties;
            }

            SerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }

        /*
            `returning => true;`
            this serializer can take any log level, state and
            exception and serialize it to be posted to the provider
        */
        public bool IsSerializable<TState>
            (LogLevel logLevel, TState state, Exception exception)
                => true;

        public (IDictionary<string, object> serializedLog, DateTimeOffset timestamp) Serialize<TState>(string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var Detail = new Dictionary<string, object>
            {
                { "level", logLevel },
                { "logger", loggerName },
                { "message", formatter(state, exception) }
            };

            var context = JsonConvert.SerializeObject(state, SerializerSettings);

            if (!string.IsNullOrWhiteSpace(context))
            {
                Detail
                    .Add("context", context);
            }

            var template = GetTemplate(state);

            if (!string.IsNullOrWhiteSpace(template))
            {
                Detail
                    .Add("template", template);
            }

            var timestamp = DateTimeOffset.UtcNow;

            var logDictionary = new Dictionary<string, object>
            {
                { $"@{nameof(timestamp).ToLower()}", timestamp.ToString("O") },
                { nameof(Detail).ToLower(), Detail },

            };

            if(Properties != null)
            {
                logDictionary.Add(
                    nameof(Properties).ToLower(), Properties
                );
            }

            return (logDictionary, timestamp);
        }

        /*
            Order by key of 0, get serializers in descending order
            to have this one come last.
        */
        public object OrderByKey => 0;

        private string GetTemplate<TState>(TState state)
        {
            if (state is IEnumerable<KeyValuePair<string, object>>)
            {
                var keyValueState = (IEnumerable<KeyValuePair<string, object>>)state;

                foreach (var item in keyValueState)
                {
                    if (item.Key.Equals("{originalformat}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return item.Value.ToString();
                    }
                }

            }
            return "";
        }
    }
}