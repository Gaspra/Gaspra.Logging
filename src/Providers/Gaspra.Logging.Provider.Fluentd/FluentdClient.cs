using Gaspra.Logging.Provider.Fluentd.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Gaspra.Logging.Provider.Fluentd.Interfaces;
using Gaspra.Logging.Provider.Fluentd.Models;
using Gaspra.Logging.Provider.Extensions;

namespace Gaspra.Logging.Provider.Fluentd
{
    public class FluentdClient : IProviderClient
    {
        private readonly IProviderPacker packer;
        private readonly IFluentdOptions options;
        private readonly IFluentdClientTimer timer;
        private readonly object syncObj;
        private ICollection<FluentdLog> sendBatch;
        private ICollection<FluentdLog> logEvents;
        private TimeSpan quietTime;
        private bool connected = false;

        public FluentdClient(
            IProviderPacker packer,
            IFluentdOptions options,
            IFluentdClientTimer timer)
        {
            this.packer = packer;
            this.options = options;
            this.timer = timer;

            syncObj = new object();
            quietTime = TimeSpan.Zero;

            this.timer.SetupTimer(new TimerCallback(async (target) =>
            {
                if (logEvents != null && logEvents.Any())
                {
                    await FlushEvents();

                    quietTime = TimeSpan.Zero;
                }
                else
                {
                    if (connected)
                    {
                        quietTime += options.FlushTime;

                        if (quietTime >= options.DisconnectTime)
                        {
                            ConsoleColor.Yellow.OutputMessage($"{typeof(FluentdClient).FullName} {nameof(timer)} -> No logs for: {quietTime} disposing client. On thread with id `{Thread.CurrentThread.ManagedThreadId}`"
                                , debug: options.Debug.On
                                , path: options.Debug.Path);

                            Dispose();
                        }
                    }
                }
            }), options.FlushTime);
        }

        public Task Send(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            if(logEvents == null)
            {
                logEvents = new List<FluentdLog>();
            }

            /*
                Add log to the logEvents collection, if the collection grows
                past the FlushSize limit the flushTimer will be invoked
            */
            var addLogTask = Task.Run(() =>
            {
                logEvents.Add(new FluentdLog(log, timestamp));

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

                if (sendBatch != null && sendBatch.Any())
                {
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

                        connected = true;
                    }
                    catch (Exception ex)
                    {
                        ConsoleColor.Red.OutputMessage($"{typeof(FluentdClient).FullName} {nameof(FlushEvents)} -> Failed sending the batch of logs due to (logs will be put back on queue): {ex.Message} {Environment.NewLine} {ex.StackTrace}"
                            , debug: options.Debug.On
                            , path: options.Debug.Path);

                        foreach (var log in sendBatch)
                        {
                            logEvents.Add(log);
                        }
                    }
                    finally
                    {
                        ConsoleColor.White.OutputMessage($"{typeof(FluentdClient).FullName} {nameof(FlushEvents)} -> Clearing sendBatch: {sendBatch.Count()} in sendBatch, and {logEvents.Count()} in logEvents still to deliver"
                            , debug: options.Debug.On
                            , path: options.Debug.Path);

                        sendBatch.Clear();
                    }
                }
            }
        }

        public void Dispose()
        {
            packer.Dispose();
            connected = false;
        }
    }
}
