using Gaspra.Logging.Serializer;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Provider.Console.Interfaces
{
    public interface IConsoleProviderOptions
    {
        public IEnumerable<Type> AppropriateSerializers { get; set; }
        public string ConsoleFormat { get; set; }
        public bool ShortLoggerName { get; set; }

    }
}
