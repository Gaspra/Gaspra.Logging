using Microsoft.Extensions.Hosting;
using System;

namespace Gaspra.Logging.ApplicationInformation.Extensions
{
    public static class PropertyRetrieverExtensions
    {
        public static string GetMachineName()
        {
            return Environment.MachineName;
        }

        public static string GetEnvironment(this IHostingEnvironment hosting)
        {
            return hosting.EnvironmentName.ToUpper();
        }

        public static string GetInstance(this IHostingEnvironment hosting)
        {
            return hosting.ApplicationName;
        }
    }
}
