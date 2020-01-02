using Gaspra.Logging.Sample.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gaspra.Logging.Sample
{
    public class LoggingSampleService : IHostedService
    {
        private readonly IEnumerable<ILoggerExample> examples;

        public LoggingSampleService(IEnumerable<ILoggerExample> examples)
        {
            this.examples = examples;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Starting: {nameof(LoggingSampleService)} - {DateTime.Now}");

            while (!await RunToCompletion()) { }

            await StopAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Stopping: {nameof(LoggingSampleService)} - {DateTime.Now}");
        }

        public async Task<bool> RunToCompletion()
        {
            Console.WriteLine("How many iterations of logs do you wish to be logged?");

            var logsToSendInput = Console.ReadLine();

            if(int.TryParse(logsToSendInput, out var logsToSend))
            {
                for(int l = 0; l < logsToSend; l++)
                {
                    foreach(var example in examples)
                    {
                        await example.DoLogs();
                    }
                }
            }

            return false;
        }
    }
}
