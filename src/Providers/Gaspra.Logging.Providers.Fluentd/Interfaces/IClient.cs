using Gaspra.Logging.Providers.Fluentd.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gaspra.Logging.Providers.Fluentd.Interfaces
{
    public interface IClient : IDisposable
    {
        void Send(IDictionary<string, object> log, DateTimeOffset timestamp);
        Task FlushEvents();
        ICollection<FluentdLog> LogEvents { get; }
    }
}
