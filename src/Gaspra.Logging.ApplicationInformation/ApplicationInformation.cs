using Gaspra.Logging.ApplicationInformation.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace Gaspra.Logging.ApplicationInformation
{
    public class ApplicationInformation : IApplicationInformation
    {
        public IDictionary<string, string> Information { get; }

        private static string InformationSection => "Logging:ApplicationInformation";

        public ApplicationInformation(
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration = null)
        {
            var information = new Dictionary<string, string>();

            /*
                Retrieve information from the configuration if the
                configuration and appropriate section are present
            */
            if (configuration != null)
            {
                if (configuration.TryGetSection(InformationSection, out var informationSection))
                {
                    information.AddSection(informationSection);
                }
            }

            /*
                If the following properties are still empty after reading
                configuration, populate using the environment and hosting environment
            */
            if (!information.ContainsKey("environment"))
                information.Add("environment", PropertyRetrieverExtensions.GetEnvironment(hostingEnvironment));

            if (!information.ContainsKey("machine"))
                information.Add("machine", PropertyRetrieverExtensions.GetMachineName());

            if (!information.ContainsKey("instance"))
                information.Add("instance", PropertyRetrieverExtensions.GetInstance(hostingEnvironment));

            Information = information;
        }
    }
}
