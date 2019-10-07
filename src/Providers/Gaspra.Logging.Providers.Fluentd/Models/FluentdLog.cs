using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Providers.Fluentd.Models
{
    public class FluentdLog
    {
        public DateTimeOffset Timestamp { get; }
        public IDictionary<string, object> Log { get; }
        public Guid CorrelationId { get; }

        public FluentdLog(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            Log = log;
            Timestamp = timestamp;
            CorrelationId = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is FluentdLog))
            {
                return false;
            }
            else
            {
                return CorrelationId
                    .Equals(
                        ((FluentdLog)obj).CorrelationId);
            }
        }

        public override int GetHashCode()
        {
            return CorrelationId
                .GetHashCode();
        }

        public override string ToString()
        {
            var logString = "";
            foreach (KeyValuePair<string, object> entry in Log)
            {
                logString += $"({entry.Key},{entry.Value})";
            }
            return $"[{CorrelationId}][{Timestamp.ToString("yyyy-MM-dd HH:mm:ss K")}][{logString}]";
        }
    }
}
