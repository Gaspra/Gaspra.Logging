using Gaspra.Logging.Provider.File.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaspra.Logging.Provider.File
{
    public class FilePacker : IFilePacker
    {
        private readonly IFileProviderOptions options;

        private string CurrentFileName;

        public FilePacker(IFileProviderOptions options)
        {
            this.options = options;
        }

        public async Task SendBatch(IEnumerable<(IDictionary<string, object> log, DateTimeOffset timestamp)> logEvents)
        {
            if(logEvents.Any())
            {
                var filePath = GetRollingFile();

                //todo: improve this (should render all then write in one async, or create a list of tasks to wait for? {could risk logs not being in order})
                var toWrite = "";

                foreach(var (log, timestamp) in logEvents)
                {
                    toWrite += RenderedText(log, timestamp);
                }

                await System.IO.File.AppendAllTextAsync(filePath, toWrite);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private string RenderedText(IDictionary<string, object> log, DateTimeOffset timestamp)
        {
            var text = $"{Environment.NewLine}[{timestamp.ToString("y-MM-dd HH:mm:ss.ffffffK")}]: ";

            foreach(var item in log)
            {
                text += $"{Environment.NewLine}{item.Key}: {item.Value}";
            }

            return text;
        }

        private string GetRollingFile()
        {
            var expectedFileName = $"{options.FileNamePrefix}{options.ApplicationName}{DateTimeOffset.UtcNow.ToString("y.MM.dd")}";

            if ((!string.IsNullOrWhiteSpace(CurrentFileName))
                && (
                    (!CurrentFileName.Equals(expectedFileName))
                ||  (ShouldArchiveFile($"{options.RootPath}/{CurrentFileName}.txt"))
                ))
            {
                System.IO.File.Move($"{options.RootPath}/{CurrentFileName}.txt", $"{options.RootPath}/{CurrentFileName}-Archived-{DateTimeOffset.UtcNow.ToString("HH.mm.ss")}.txt");
            }

            CurrentFileName = expectedFileName;

            return $"{options.RootPath}/{CurrentFileName}.txt";
        }

        private bool ShouldArchiveFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                long length = new FileInfo(path).Length;

                float logFileLengthMb = (float)length / 1000000;

                if(logFileLengthMb >= options.RollingFileSizeMb)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
