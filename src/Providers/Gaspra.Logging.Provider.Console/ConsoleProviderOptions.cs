using Gaspra.Logging.Provider.Console.Interfaces;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleProviderOptions : IConsoleProviderOptions
    {
        public string ConsoleFormat { get; set; }
        public bool ShortLoggerName { get; set; }

        public ConsoleProviderOptions()
        {
            //todo: put message into this, only colour in between the brackets
            ConsoleFormat = "[timestamp level name]: ";

            ShortLoggerName = true;
        }
    }
}
