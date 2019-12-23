using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using test.Models;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            while(true)
            {
                Log();

                System.Threading.Thread.Sleep(2000);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void Log()
        {
            logger.LogDebug("I'm a debug message being logged");
            logger.LogInformation("I'm an info message being logged");
            logger.LogWarning("I'm an warn message being logged");
            logger.LogError("I'm an error message being logged");
            logger.LogCritical("I'm an critical message being logged");
        }
    }
}
