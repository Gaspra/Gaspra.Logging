using Gaspra.Logging.Provider.Console.Extensions;
using Gaspra.Logging.Provider.Console.Interfaces;
using Gaspra.Logging.Provider.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleLogger : IConsoleLogger
    {
        private readonly IConsoleOptions options;

        public ConsoleLogger(IConsoleOptions options)
        {
            this.options = options;
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
                LogFormattedMessage(logLevel, DateTimeOffset.UtcNow, formatter(state, exception));
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.OutputMessage($"Unable to log message at {DateTimeOffset.UtcNow}, due to {ex.Message}.{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public void Dispose()
        {

        }

        private void LogFormattedMessage(LogLevel level, DateTimeOffset timestamp, string message)
        {
            var (back, fore) = LevelColour(level);
            $"[{timestamp.ToString("HH:mm:ss.fff")} {ShortLevel(level)}]".OutputMessage(back, fore);
            $"[{Name}]:".OutputMessage(back, fore);
            $" {message}".OutputMessage(lineEnding:true);
        }

        private (ConsoleColor back, ConsoleColor fore) LevelColour(LogLevel logLevel)
        {
            switch(logLevel)
            {
                case LogLevel.Debug:
                    return (ConsoleColor.DarkGreen, ConsoleColor.White);

                case LogLevel.Information:
                    return (ConsoleColor.DarkBlue, ConsoleColor.White);

                case LogLevel.Warning:
                    return (ConsoleColor.DarkYellow, ConsoleColor.White);

                case LogLevel.Error:
                    return (ConsoleColor.DarkRed, ConsoleColor.White);

                case LogLevel.Critical:
                    return (ConsoleColor.DarkMagenta, ConsoleColor.White);

                default:
                    return (ConsoleColor.DarkGray, ConsoleColor.White);
            }
        }

        private string ShortLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    return "DBG";

                case LogLevel.Information:
                    return "INF";

                case LogLevel.Warning:
                    return "WRN";

                case LogLevel.Error:
                    return "ERR";

                case LogLevel.Critical:
                    return "CRT";

                default:
                    return "LOG";
            }
        }
    }
}
