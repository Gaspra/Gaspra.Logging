﻿using Gaspra.Logging.Provider.Console.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Provider.Console.Serializer
{
    public class ConsoleSerializer : IConsoleLogSerializer
    {
        public object OrderByKey => 0;

        public bool IsSerializable<TState>(LogLevel logLevel, TState state, Exception exception)
            => true;

        public (IDictionary<string, object> serializedLog, DateTimeOffset timestamp) Serialize<TState>(string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var serializedLog = new Dictionary<string, object>
            {
                { "message", formatter(state, exception) }
            };

            return (serializedLog, DateTimeOffset.UtcNow);
        }

    }
}
