using Gaspra.Logging.Sample.Interfaces;
using Gaspra.Logging.Sample.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Gaspra.Logging.Sample.Implementation
{
    public class LoggerCDuplicate : ILoggerExample
    {
        private readonly ILogger logger;

        public LoggerCDuplicate(ILogger<LoggerC> logger)
        {
            this.logger = logger;
        }

        public async Task DoLogs()
        {
            await Task.Run(() =>
            {
                var levelsDeep = 5;
                logger.LogDebug("Building a complex object ({levelsDeep} deep) from scratch: {example}", levelsDeep, new ComplexLoggingObject(levelsDeep));
                logger.LogInformation("Logging information with correlationid: {guid}", Guid.NewGuid());
                logger.LogWarning("Logging a warning with a pre-existing complex object {example}", new ComplexLoggingObject(3));
                logger.LogError(new Exception("I'm an exception, bang!"), "Testing if things go bang!");
                logger.LogCritical("I'm a critical warning, uh oh!");
            });
        }
    }
}
