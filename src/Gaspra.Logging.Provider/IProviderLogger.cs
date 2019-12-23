using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider
{
    public interface IProviderLogger : ILogger, IDisposable
    {
        string Name { get; set; }
    }
}
