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
    }
}
