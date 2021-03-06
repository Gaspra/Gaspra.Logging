﻿using Gaspra.Logging.Provider.Console.Extensions;
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
        private readonly IConsoleProviderOptions options;
        private readonly IEnumerable<IConsoleLogSerializer> serializers;

        public ConsoleLogger(IConsoleProviderOptions options, IEnumerable<IConsoleLogSerializer> serializers)
        {
            this.options = options;
            this.serializers = serializers;
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

                var (serializedLog, timestamp) = logSerializer.Serialize(Name, logLevel, eventId, state, exception, formatter);

                var (back, fore) = logLevel.ConsoleColour();

                options.ConsoleFormat
                    .Replace("timestamp", timestamp.ToString("HH:mm:ss.fff"))
                    .Replace("level", logLevel.ShortString())
                    .Replace("name", options.ShortLoggerName ? Name.Split(".").Last() : Name)
                    .OutputMessage(back, fore);

                $" {string.Join(", ", serializedLog.Values)}"
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
