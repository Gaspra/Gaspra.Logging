using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Serializer
{
    public class SimpleSerializer : ILogSerializer
    {
        public object OrderByKey => throw new NotImplementedException();

        public bool IsSerializable<TState>(LogLevel logLevel, TState state, Exception exception)
        {
            throw new NotImplementedException();
        }

        public (IDictionary<string, object> serializedLog, DateTimeOffset timestamp) Serialize<TState>(string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
