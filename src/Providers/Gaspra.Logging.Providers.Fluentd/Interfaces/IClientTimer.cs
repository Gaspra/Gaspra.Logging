using System;
using System.Threading;

namespace Gaspra.Logging.Providers.Fluentd.Interfaces
{
    public interface IClientTimer
    {
        void SetupTimer(TimerCallback callback, TimeSpan trigger);
        void UpdateInterval(TimeSpan interval, bool resetTimer = true);
    }
}
