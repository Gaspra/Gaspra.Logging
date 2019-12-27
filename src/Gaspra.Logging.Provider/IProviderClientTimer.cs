using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Gaspra.Logging.Provider
{
    public interface IProviderClientTimer
    {
        void SetupTimer(TimerCallback callback, TimeSpan trigger);
        void UpdateInterval(TimeSpan interval, bool resetTimer = true);
    }
}
