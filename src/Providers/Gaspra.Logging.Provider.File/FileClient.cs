using Gaspra.Logging.Provider.Extensions;
using Gaspra.Logging.Provider.File.Interfaces;
using Gaspra.Logging.Provider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Gaspra.Logging.Provider.File
{
    public class FileClient : IFileClient
    {
        private readonly IFilePacker packer;
        private readonly IFileProviderOptions options;
        private readonly IFileClientTimer timer;
        private IList<SerializedLog> logEvents;
        private bool flushing = false;

        public FileClient(
            IFilePacker packer,
            IFileProviderOptions options,
            IFileClientTimer timer)
        {
            this.packer = packer;
            this.options = options;
            this.timer = timer;

            this.timer.SetupTimer(new TimerCallback(async (target) =>
            {
                if (!flushing)
                {
                    if (logEvents != null && logEvents.Any())
                    {
                        await FlushEvents();
                    }
                }
            }), options.FlushTime);
        }

        public async Task Send(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            if (logEvents == null)
            {
                logEvents = new List<SerializedLog>();
            }

            /*
                Add log to the logEvents collection, if the collection grows
                past the FlushSize limit the flushTimer will be invoked
            */

            logEvents.Add(new SerializedLog(log, timestamp));

            if (!flushing && logEvents.Count() > options.FlushSize)
            {
                timer
                    .UpdateInterval(options.FlushTime, false);
            }

        }

        public async Task FlushEvents()
        {
            if(!flushing)
            {
                flushing = true;

                var toSend = logEvents.Take(options.FlushSize);

                try
                {
                    await packer
                        .SendBatch(toSend
                            .Select(l => (l.Log, l.Timestamp)));
                }
                catch (Exception ex)
                {
                    ConsoleColor.Red.OutputMessage($"{typeof(FileClient).FullName} {nameof(FlushEvents)} -> Failed sending the batch of logs due to: {ex.Message} {Environment.NewLine} {ex.StackTrace}");

                    toSend = null;
                }

                if(toSend != null)
                {
                    foreach(var log in toSend)
                    {
                        logEvents.Remove(log);
                    }
                }

                flushing = false;
            }
        }

        public void Dispose()
        {
            packer.Dispose();
        }
    }
}
