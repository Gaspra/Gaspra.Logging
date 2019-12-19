using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Serializer
{
    public interface ILogSerializer
    {
        /// <summary>
        /// Determines whether the log serializer is appropriate for the provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        bool IsProviderAppropriate
            (string providerName);

        /// <summary>
        /// Determines whether this serializer can serialize the log event.
        /// Log serializer could contextually only be correct for certain
        /// log levels, or it could depend on the objects in the state/ exception.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool IsSerializable<TState>
            (LogLevel logLevel, TState state, Exception exception);

        /// <summary>
        /// Takes the log event and serializes it to a IDictionary<string, object>
        /// which can be packed appropriately. Also returns a timestamp at the time
        /// of creation.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="loggerName"></param>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        (IDictionary<string, object> serializedLog, DateTimeOffset timestamp) Serialize<TState>
                (string loggerName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);

        /// <summary>
        /// Generic object to order the serializer by when there are
        /// multiple serializers and ordering alphabetically doesn't
        /// yield the correct results
        /// </summary>
        object OrderByKey { get; }
    }
}
