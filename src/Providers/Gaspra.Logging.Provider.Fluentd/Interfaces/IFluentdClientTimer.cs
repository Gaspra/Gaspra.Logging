using System;
using System.Threading;

namespace Gaspra.Logging.Provider.Fluentd.Interfaces
{
    public interface IFluentdClientTimer
    {
        void SetupTimer(TimerCallback callback, TimeSpan trigger);
        void UpdateInterval(TimeSpan interval, bool resetTimer = true);
    }
}
