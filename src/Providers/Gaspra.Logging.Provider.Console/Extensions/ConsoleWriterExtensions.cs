using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider.Console.Extensions
{
    public static class ConsoleWriterExtensions
    {
        public static void OutputMessage(
                this string message,
                ConsoleColor background = ConsoleColor.Black,
                ConsoleColor foreground = ConsoleColor.Gray,
                bool lineEnding = false
            )
        {
            System.Console.BackgroundColor = background;
            System.Console.ForegroundColor = foreground;

            if (lineEnding)
            {
                System.Console.WriteLine(message);
            }
            else
            {
                System.Console.Write(message);
            }
        }

        public static (ConsoleColor back, ConsoleColor fore) ConsoleColour(this LogLevel logLevel)
        {
            switch (logLevel)
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

        public static string ShortString(this LogLevel logLevel)
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
