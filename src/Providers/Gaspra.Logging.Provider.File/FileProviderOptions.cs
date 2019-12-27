using Gaspra.Logging.Provider.File.Interfaces;
using System;

namespace Gaspra.Logging.Provider.File
{
    public class FileProviderOptions : IFileProviderOptions
    {
        public TimeSpan FlushTime { get; set; } = TimeSpan.FromSeconds(30);
        public int FlushSize { get; set; } = 5000;
        public float RollingFileSizeMb { get; set; } = 1.5f;
        public bool RollByDay { get; set; } = true;
        public string ApplicationName { get; set; }
        public string RootPath { get; set; }
        public string FileNamePrefix { get; set; } = "Logs.";

        public FileProviderOptions(string applicationName, string rootPath)
        {
            ApplicationName = applicationName;
            RootPath = rootPath;
        }
    }
}
