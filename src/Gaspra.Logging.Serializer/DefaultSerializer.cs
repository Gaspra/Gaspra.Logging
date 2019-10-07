using Gaspra.Logging.ApplicationInformation;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Serializer
{
    public class DefaultSerializer : ILogSerializer
    {
        private JsonSerializerSettings SerializerSettings { get; }

        private string Tag { get; set; }
        private IDictionary<string, string> Properties { get; set; }

        public DefaultSerializer(IApplicationInformation appInfo)
        {
            /*
                Properties are not expected to change after serializer is
                instantiated. This model doesn't fit for multitenanted
                services which might serve multiple clients from a single
                instance.
            */
            Properties = appInfo.Information;

            Tag = TryBuildTag(appInfo);

            /*
                To make the default serialization as safe as possible it'll
                ignore self referencing loops (HttpContext is a big offender
                of this)
            */
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
                { nameof(Properties).ToLower(), Properties },
                { nameof(Tag).ToLower(), Tag }
            };

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

        private string TryBuildTag(IApplicationInformation appInfo)
        {
            var tagList = new List<string>();

            if (appInfo.Information.TryGetValue("environment", out var environment))
                tagList.Add(environment);

            if (appInfo.Information.TryGetValue("system", out var system))
                tagList.Add(system);

            if (appInfo.Information.TryGetValue("client", out var client))
                tagList.Add(client);

            if (appInfo.Information.TryGetValue("instance", out var instance))
                tagList.Add(instance);

            var tag = string.Join(".", tagList).ToUpper();

            return tag;
        }
    }
}