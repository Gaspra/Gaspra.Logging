using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Gaspra.Logging.Builder;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Gaspra.Logging.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
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
                    .AddProviderConsole();
            })
            .ConfigureServices((host, services) =>
            {
                services
                    .AddHostedService<LoggingSampleService>();
            });
    }
}
