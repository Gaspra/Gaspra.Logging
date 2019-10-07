using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gaspra.Logging.Sample.Default
{
    public class Startup
    {
        private readonly ILogger logger;

        public Startup(ILogger<Startup> logger)
        {
            this.logger = logger;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            logger.LogInformation("Testing logging default setup: {name}", "Gaspra");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /*do nothing, configure services will run before and log*/
        }
    }
}
