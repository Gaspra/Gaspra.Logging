using Gaspra.Logging.Provider.File.Interfaces;
using System;
using System.Threading;

namespace Gaspra.Logging.Provider.File
{
    public class FileClientTimer : IFileClientTimer
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
