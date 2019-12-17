using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gaspra.Logging.Provider
{
    public interface ProviderClient : IDisposable
    {
        Task Send(IDictionary<string, object> log, DateTimeOffset timestamp);
        Task FlushEvents();
    }
}
