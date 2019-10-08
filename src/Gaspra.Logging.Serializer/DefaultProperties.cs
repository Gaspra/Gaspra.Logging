using Gaspra.Logging.Serializer.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Serializer
{
    public class DefaultProperties : ILogProperties
    {
        public IDictionary<string, string> Properties { get; set; }

        private static string PropertiesSection => "Logging:Properties";

        public DefaultProperties(
            IHostEnvironment hostEnvironment,
            IConfiguration configuration = null)
        {
            var properties = new Dictionary<string, string>();

            /*
                Retrieve information from the configuration if the
                configuration and appropriate section are present
            */
            if (configuration != null)
            {
                if (configuration.TryGetSection(PropertiesSection, out var propertiesSection))
                {
                    properties.AddSection(propertiesSection);
                }
            }

            /*
                If the following properties are still empty after reading
                configuration, populate using the environment and hosting environment
            */
            if (!properties.ContainsKey("environment"))
                properties.Add("environment", hostEnvironment.EnvironmentName);

            if (!properties.ContainsKey("machine"))
                properties.Add("machine", Environment.MachineName);

            if (!properties.ContainsKey("instance"))
                properties.Add("instance", hostEnvironment.ApplicationName);

            Properties = properties;
        }
    }
}
