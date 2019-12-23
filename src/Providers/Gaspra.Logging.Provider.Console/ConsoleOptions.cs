using Gaspra.Logging.Provider.Console.Interfaces;
using Gaspra.Logging.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Provider.Console
{
    public class ConsoleOptions : IConsoleOptions
    {
        public IEnumerable<Type> AppropriateSerializers { get; set; }
        public string ConsoleFormat { get; set; }

        public ConsoleOptions()
        {
            AppropriateSerializers = new List<Type>
            {
                typeof(SimpleSerializer)
            };

            ConsoleFormat = "[{timestamp} {level}][{name}]: {message}";
        }
    }
}
