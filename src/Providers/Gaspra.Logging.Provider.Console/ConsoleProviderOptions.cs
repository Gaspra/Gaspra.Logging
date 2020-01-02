using Gaspra.Logging.Provider.Console.Interfaces;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleProviderOptions : IConsoleProviderOptions
    {
        public string ConsoleFormat { get; set; }
        public bool ShortLoggerName { get; set; }

        public ConsoleProviderOptions()
        {
            ConsoleFormat = "[timestamp level name]: message";

            ShortLoggerName = true;
        }
    }
}
