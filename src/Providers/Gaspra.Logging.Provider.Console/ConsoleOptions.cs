using Gaspra.Logging.Provider.Console.Interfaces;
using Gaspra.Logging.Provider.Console.Serializer;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleOptions : IConsoleOptions
    {
        public IEnumerable<Type> AppropriateSerializers { get; set; }
        public string ConsoleFormat { get; set; }
        public bool ShortLoggerName { get; set; }

        public ConsoleOptions()
        {
            AppropriateSerializers = new List<Type>
            {
                typeof(ConsoleSerializer)
            };

            ConsoleFormat = "[timestamp level name]:";

            ShortLoggerName = true;
        }
    }
}
