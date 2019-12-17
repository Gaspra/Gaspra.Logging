using Gaspra.Logging.Provider.Fluentd.Interfaces;
using Gaspra.Logging.Provider.Fluentd.Extensions;
using MsgPack;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Gaspra.Logging.Provider.Fluentd
{
    public class LogPacker : ILogPacker
    {
        private TcpClient tcpClient;
        private readonly SerializationContext serializationContext;
        private readonly IOptions options;

        public LogPacker(IOptions options)
        {
            this.options = options;

            serializationContext = new SerializationContext(PackerCompatibilityOptions.PackBinaryAsRaw)
            {
                SerializationMethod = SerializationMethod.Map
            };
        }

        public async Task SendBatch(IEnumerable<(IDictionary<string, object> log, DateTimeOffset timestamp)> logEvents)
        {
            var retryLimit = options.ConnectionRetryLimit;
            while (!IsConnected())
            {
                await Connect();
                retryLimit--;

                if(retryLimit <= 0)
                {
                    Dispose();
                    throw new Exception($"Method: {nameof(SendBatch)} was unable to connect, cant send log batch");
                }
            }

            using (var sw = new MemoryStream())
            {
                var packer = Packer.Create(sw);

                foreach (var (log, timestamp) in logEvents)
                {
                    var unixTimestamp = (ulong)timestamp.ToUnixTimeSeconds();
                    var hasTag = log.TryGetValue("tag", out var tag);

                    await packer.PackArrayHeaderAsync(3);

                    await packer.PackStringAsync(hasTag ? tag.ToString() : "", Encoding.UTF8);
                    await packer.PackAsync(unixTimestamp);
                    await packer.PackAsync(log, serializationContext);
                }

                var stream = tcpClient.GetStream();
                var data = sw.ToArray();
                await stream.WriteAsync(data, 0, data.Length);
                await stream.FlushAsync();
            }
        }

        private bool IsConnected()
        {
            return
                tcpClient != null &&
                tcpClient.Connected &&
                tcpClient.GetStream().CanWrite;
        }

        private async Task Connect()
        {
            tcpClient = new TcpClient
            {
                NoDelay = options.NoDelay,
                SendTimeout = (int)options.SendTimeout.TotalMilliseconds,
                LingerState = new LingerOption(options.LingerEnabled, (int)options.LingerTime.TotalSeconds)
            };

            try
            {
                await tcpClient.ConnectAsync(options.Host, options.Port);
            }
            catch (Exception ex)
            {
                ConsoleColor.Red.OutputMessage($"{typeof(LogPacker).FullName} {nameof(Connect)} -> failed due to: {ex.Message} {Environment.NewLine} {ex.StackTrace}"
                    , debug: options.Debug.On
                    , path: options.Debug.Path);

                tcpClient.Dispose();
            }
        }

        public void Dispose()
        {
            if (IsConnected())
            {
                tcpClient.Dispose();
            }

            tcpClient = null;
        }
    }
}
