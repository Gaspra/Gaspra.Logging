using Gaspra.Logging.Providers.Fluentd.Interfaces;
using System;
using System.Threading;

namespace Gaspra.Logging.Providers.Fluentd
{
    public class ClientTimer : IClientTimer
    {
        private Timer timer;

        public void SetupTimer(TimerCallback callback, TimeSpan trigger)
        {
            timer = new Timer(callback);
            UpdateInterval(trigger, true);
        }

        public void UpdateInterval(TimeSpan interval, bool resetTimer = true)
        {
            timer.Change(resetTimer ? interval : TimeSpan.Zero, interval);
        }
    }
}
