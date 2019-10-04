using Gaspra.Logging.Providers.Fluentd.Static;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using Gaspra.Logging.Providers.Fluentd.Interfaces;
using Gaspra.Logging.Providers.Fluentd.Models;

namespace Gaspra.Logging.Providers.Fluentd
{
    public class Client : IClient
    {
        private readonly ILogPacker packer;
        private readonly IOptions options;
        private readonly IClientTimer timer;
        private readonly object syncObj;
        private ICollection<FluentdLog> sendBatch;
        private TimeSpan quietTime;
        private bool connected = false;

        public ICollection<FluentdLog> LogEvents { get; private set; }

        public Client(
            ILogPacker packer,
            IOptions options,
            IClientTimer timer)
        {
            this.packer = packer;
            this.options = options;
            this.timer = timer;

            syncObj = new object();
            quietTime = TimeSpan.Zero;

            this.timer.SetupTimer(new TimerCallback(async (target) =>
            {
                if (LogEvents != null && LogEvents.Any())
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
                            ConsoleColor.Yellow.OutputMessage($"{typeof(Client).FullName} {nameof(timer)} -> No logs for: {quietTime} disposing client. On thread with id `{Thread.CurrentThread.ManagedThreadId}`"
                                , debug: options.Debug.On
                                , path: options.Debug.Path);

                            Dispose();
                        }
                    }
                }
            }), options.FlushTime);
        }

        public void Send(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            if(LogEvents == null)
            {
                LogEvents = new List<FluentdLog>();
            }

            /*
                Add log to the logEvents collection, if the collection grows
                past the FlushSize limit the flushTimer will be invoked
            */
            LogEvents.Add(new FluentdLog(log, timestamp));

            if(LogEvents.Count() > options.FlushSize)
            {
                timer
                    .UpdateInterval(options.FlushTime, false);
            }
        }

        public async Task FlushEvents()
        {
            if (LogEvents != null && LogEvents.Any())
            {
                lock (syncObj)
                {
                    /*
                        Use a single thread to create a batch of log events
                        to send to the packer
                    */
                    var batch = LogEvents.ToList();

                    foreach (var log in batch)
                    {
                        LogEvents.Remove(log);
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
                        ConsoleColor.Red.OutputMessage($"{typeof(Client).FullName} {nameof(FlushEvents)} -> Failed sending the batch of logs due to (logs will be put back on queue): {ex.Message} {Environment.NewLine} {ex.StackTrace}"
                            , debug: options.Debug.On
                            , path: options.Debug.Path);

                        foreach (var log in sendBatch)
                        {
                            LogEvents.Add(log);
                        }
                    }
                    finally
                    {
                        ConsoleColor.White.OutputMessage($"{typeof(Client).FullName} {nameof(FlushEvents)} -> Clearing sendBatch: {sendBatch.Count()} in sendBatch, and {LogEvents.Count()} in logEvents still to deliver"
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
