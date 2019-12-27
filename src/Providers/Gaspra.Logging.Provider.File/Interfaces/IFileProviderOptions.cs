using System;

namespace Gaspra.Logging.Provider.File.Interfaces
{
    public interface IFileProviderOptions
    {
        TimeSpan FlushTime { get; set; }
        int FlushSize { get; set; }
        float RollingFileSizeMb { get; set; }
        public bool RollByDay { get; set; }
        public string ApplicationName { get; set; }
        public string RootPath { get; set; }
        public string FileNamePrefix { get; set; }
    }
}
