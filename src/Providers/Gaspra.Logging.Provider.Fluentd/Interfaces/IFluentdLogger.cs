using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Provider.Fluentd.Interfaces
{
    public interface IFluentdLogger : ILogger, IDisposable
    {
        string Name { get; set; }
    }
}
