using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Gaspra.Logging.Builder;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Gaspra.Logging.Provider.File;
using System.IO;

namespace Gaspra.Logging.Sample
{
    public class Program
    {
        private static string logDirectory = $"{Directory.GetCurrentDirectory()}/Logs";

        public static async Task Main(string[] args)
        {
            Directory.CreateDirectory(logDirectory);

            await CreateHostBuilder(args)
                .RunConsoleAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureLogging((host, logger) =>
            {
                logger
                    .ClearProviders()
                    .SetMinimumLevel(LogLevel.Debug)
                    .AddProviderConsole()
                    .AddProviderFileWithOptions(new FileProviderOptions("Gaspra.Logging.Sample", logDirectory));
            })
            .ConfigureServices((host, services) =>
            {
                services
                    .AddHostedService<LoggingSampleService>();
            });
    }
}
