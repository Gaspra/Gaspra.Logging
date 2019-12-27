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
        private readonly object syncObj;
        private ICollection<SerializedLog> sendBatch;
        private IList<SerializedLog> logEvents;
        private bool syncing = false;

        public FileClient(
            IFilePacker packer,
            IFileProviderOptions options,
            IFileClientTimer timer)
        {
            this.packer = packer;
            this.options = options;
            this.timer = timer;

            syncObj = new object();

            this.timer.SetupTimer(new TimerCallback(async (target) =>
            {
                if (logEvents != null && logEvents.Any())
                {
                    await FlushEvents();
                }
            }), options.FlushTime);
        }

        public Task Send(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            if (logEvents == null)
            {
                logEvents = new List<SerializedLog>();
            }

            /*
                Add log to the logEvents collection, if the collection grows
                past the FlushSize limit the flushTimer will be invoked
            */
            var addLogTask = Task.Run(() =>
            {
                logEvents.Add(new SerializedLog(log, timestamp));

                if (logEvents.Count() > options.FlushSize)
                {
                    timer
                        .UpdateInterval(options.FlushTime, false);
                }
            });

            return addLogTask;
        }

        public async Task FlushEvents()
        {
            if (logEvents != null && logEvents.Any())
            {
                lock (syncObj)
                {
                    /*
                        Use a single thread to create a batch of log events
                        to send to the packer
                    */
                    var batch = logEvents.ToList();

                    foreach (var log in batch)
                    {
                        logEvents.Remove(log);
                    }

                    sendBatch = batch;
                }

                if (sendBatch != null && sendBatch.Any() && syncing == false)
                {
                    syncing = true;

                    try
                    {
                        /*
                            If the connection or log events send throws an exception
                            the logs are put back on the logEvents collection
                        */
                        await packer.SendBatch(
                            sendBatch.Select(f =>
                            {
                                return (f.Log, f.Timestamp);
                            })
                        );
                    }
                    catch (Exception ex)
                    {
                        ConsoleColor.Red.OutputMessage($"{typeof(FileClient).FullName} {nameof(FlushEvents)} -> Failed sending the batch of logs due to (logs will be put back on queue): {ex.Message} {Environment.NewLine} {ex.StackTrace}");

                        foreach (var log in sendBatch)
                        {
                            logEvents.Add(log);
                        }
                    }
                    finally
                    {
                        sendBatch.Clear();
                        syncing = false;
                    }
                }
            }
        }

        public void Dispose()
        {
            packer.Dispose();
        }
    }
}
