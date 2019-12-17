using System;

namespace Gaspra.Logging.Provider.Fluentd.Interfaces
{
    public interface IOptions
    {
        string Host { get; set; }
        int Port { get; set; }
        TimeSpan SendTimeout { get; set; }
        bool LingerEnabled { get; set; }
        TimeSpan LingerTime { get; set; }
        int RetryLimit { get; set; }
        bool NoDelay { get; set; }
        TimeSpan DisconnectTime { get; set; }
        (bool On, string Path) Debug { get; set; }
        TimeSpan FlushTime { get; set; }
        int FlushSize { get; set; }
        int ConnectionRetryLimit { get; set; }

        /*
            Validating the options allows you to throw
            quickly if something is not setup correctly
        */
        void Validate();
    }
}
