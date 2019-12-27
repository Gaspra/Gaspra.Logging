using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Provider.Models
{
    public class SerializedLog
    {
        public DateTimeOffset Timestamp { get; }
        public IDictionary<string, object> Log { get; }
        public Guid CorrelationId { get; }

        public SerializedLog(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            Log = log;
            Timestamp = timestamp;
            CorrelationId = Guid.NewGuid();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is SerializedLog))
            {
                return false;
            }
            else
            {
                return CorrelationId
                    .Equals(
                        ((SerializedLog)obj).CorrelationId);
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
                logString += $"{Environment.NewLine}{entry.Key}: {entry.Value}";
            }
            return $"[{Timestamp.ToString("yyyy-MM-dd HH:mm:ss K")}]:{logString}";
        }
    }
}
