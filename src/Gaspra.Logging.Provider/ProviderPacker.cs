using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gaspra.Logging.Provider
{
    public interface ProviderPacker : IDisposable
    {
        Task SendBatch(IEnumerable<(IDictionary<string, object> log, DateTimeOffset timestamp)> logEvents);
    }
}