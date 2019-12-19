using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider
{
    public interface ProviderLogger : ILogger, IDisposable
    {
        string Name { get; set; }
    }
}
