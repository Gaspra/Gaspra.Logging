using Gaspra.Logging.Providers.Fluentd.Interfaces;
using Gaspra.Logging.Providers.Fluentd.Static;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Sockets;

namespace Gaspra.Logging.Providers.Fluentd
{
    public class Options : IOptions
    {
        private static string FluentdSectionName => "Logging:Fluentd";

        public Options(IConfiguration configuration)
        {
            var fluentdConfig = configuration.GetSection(FluentdSectionName);

            //Required fields
            Host = fluentdConfig.GetString(nameof(Host));
            Port = fluentdConfig.GetInt(nameof(Port));

            //Optional fields
            SendTimeout = TimeSpan.FromMilliseconds(fluentdConfig.GetItem<double>(nameof(SendTimeout), true, 1000));
            LingerEnabled = fluentdConfig.GetItem(nameof(LingerEnabled), true, true);
            LingerTime = TimeSpan.FromMilliseconds(fluentdConfig.GetItem<double>(nameof(LingerTime), true, 1000));
            RetryLimit = fluentdConfig.GetInt(nameof(RetryLimit), true, 5);
            NoDelay = fluentdConfig.GetItem(nameof(NoDelay), true, false);
            DisconnectTime = TimeSpan.FromSeconds(fluentdConfig.GetItem<double>(nameof(DisconnectTime), true, 120));
            FlushTime = TimeSpan.FromSeconds(fluentdConfig.GetItem<double>(nameof(FlushTime), true, 2));
            FlushSize = fluentdConfig.GetInt(nameof(FlushSize), true, 500);
            ConnectionRetryLimit = fluentdConfig.GetInt(nameof(ConnectionRetryLimit), true, 3);

            //Debug output
            var debugPath = fluentdConfig.GetItem("DebugPath", true, "");
            Debug = (!string.IsNullOrWhiteSpace(debugPath), debugPath);

            //Validate options
            Validate();
        }

        public Options(string host, int port)
        {
            //Required fields
            Host = host;
            Port = port;

            //Validate options
            Validate();
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public TimeSpan SendTimeout { get; set; } = TimeSpan.FromMilliseconds(1000);
        public bool LingerEnabled { get; set; } = true;
        public TimeSpan LingerTime { get; set; } = TimeSpan.FromMilliseconds(1000);
        public int RetryLimit { get; set; } = 5;
        public bool NoDelay { get; set; } = false;
        public TimeSpan DisconnectTime { get; set; } = TimeSpan.FromSeconds(120);
        public (bool On, string Path) Debug { get; set; } = (false, "");

        public TimeSpan FlushTime { get; set; } = TimeSpan.FromSeconds(2);
        public int FlushSize { get; set; } = 500;
        public int ConnectionRetryLimit { get; set; } = 3;

        /*
            Validate the options for Fluentd, ideal for ensuring a connection exists
            at the time of startup. If the host/ port were incorrect this would blow
            up when the application starts.
        */

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Host))
            {
                throw new Exception($"{nameof(Options)}: Host can't be null or whitespace");
            }

            var client = new TcpClient();

            try
            {
                client.Connect(Host, Port);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(Options)}: Exception while connecting: {ex.Message}");
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}
