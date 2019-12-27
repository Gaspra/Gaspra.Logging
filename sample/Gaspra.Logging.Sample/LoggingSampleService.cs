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
        private readonly ILogger logger;

        public LoggingSampleService(ILogger<LoggingSampleService> logger)
        {
            this.logger = logger;
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
            Console.WriteLine("How many logs do you wish to be sent?");

            var logsToSendInput = Console.ReadLine();

            if(int.TryParse(logsToSendInput, out var logsToSend))
            {
                for(int l = 0; l < logsToSend;)
                {
                    logger.LogDebug($"Logging a message #{l++}");
                    logger.LogInformation($"Logging a message #{l++}");
                    logger.LogWarning($"Logging a message #{l++}");
                    logger.LogError($"Logging a message #{l++}");
                    logger.LogCritical($"Logging a message #{l++}");
                }
            }

            return false;
        }
    }
}
