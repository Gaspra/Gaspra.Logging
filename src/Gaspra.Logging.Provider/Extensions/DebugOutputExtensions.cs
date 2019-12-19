using System;
using System.IO;

namespace Gaspra.Logging.Provider.Extensions
{
    public static class DebugOutputExtensions
    {
        public static void OutputMessage(
            this ConsoleColor foreground,
            string message,
            ConsoleColor background = ConsoleColor.Black,
            bool debug = false,
            string path = ""
            )
        {
            /*
                Try write to console, this is useful for debugging.
                If something blows up while writing a log you might
                miss it. Starting the application as a console app
                makes it easier to spot!
            */
            try
            {
                var originalBackground = Console.BackgroundColor;
                var originalForeground = Console.ForegroundColor;
                Console.BackgroundColor = background;
                Console.ForegroundColor = foreground;
                Console.WriteLine(message);
                Console.BackgroundColor = originalBackground;
                Console.ForegroundColor = originalForeground;
            }
            catch { /*console doesn't exist*/ }

            /*
                If debug output to file is available try save the
                log message to file.
            */
            if (debug
                && !string.IsNullOrWhiteSpace(path)
                && Directory.Exists(path))
            {
                try
                {
                    var date = DateTimeOffset.UtcNow;
                    var fileName = $"{date.ToString("yMMdd")}.FluentdProvider.Log.txt";
                    var fullPath = $"{path}/{fileName}";
                    var formattedMessage = $"[{date.ToString("yyyy-MM-dd HH:mm:ss K")}]:: {message}{Environment.NewLine}";
                    File.AppendAllText(fullPath, formattedMessage);
                }
                catch { /*something went wrong saving the file*/ }
            }
        }
    }
}
