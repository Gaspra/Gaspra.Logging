using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gaspra.Logging.Builder;
using Gaspra.Logging.Provider.Console.Extensions;
using Gaspra.Logging.Provider.File.Extensions;
using Gaspra.Logging.Provider.Fluentd.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging(logger =>
                    {
                        logger.Services
                            .AddDefaultLogProperties()
                            .AddDefaultLogSerializer();

                        logger
                            .ClearProviders()
                            .AddConsoleLogger()
                            .AddFileLogger()
                            .AddFluentdLogger(new Gaspra.Logging.Provider.Fluentd.FluentdOptions("gaspra.co.uk", 9833)
                            {
                            })
                            ;
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
