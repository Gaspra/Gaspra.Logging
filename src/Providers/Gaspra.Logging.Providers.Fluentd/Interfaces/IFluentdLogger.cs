using Microsoft.Extensions.Logging;
using System;

namespace Gaspra.Logging.Providers.Fluentd.Interfaces
{
    public interface IFluentdLogger : ILogger, IDisposable
    {
        string Name { get; set; }
    }
}
