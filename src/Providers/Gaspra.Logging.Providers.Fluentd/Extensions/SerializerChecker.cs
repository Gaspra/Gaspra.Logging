using Gaspra.Logging.Serializer;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gaspra.Logging.Providers.Fluentd.Static
{
    public static class SerializerChecker
    {
        /*
            Get the first serializer appropriate to the log level, state and exception.
            Up to whatever is supplying the list to ensure they are ordered correctly.
        */
        public static ILogSerializer GetAppropriateSerializer<TState>(this IEnumerable<ILogSerializer> serializers,
            LogLevel logLevel,
            TState state,
            Exception exception)
        {
            var serializer = serializers
                .FirstOrDefault(p => p.IsSerializable(logLevel, state, exception));

            return serializer;
        }
    }
}
