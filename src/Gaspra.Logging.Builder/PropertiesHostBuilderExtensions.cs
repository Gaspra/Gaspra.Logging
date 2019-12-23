using Gaspra.Logging.Provider.Fluentd;
using Gaspra.Logging.Provider.Fluentd.Extensions;
using Gaspra.Logging.Serializer;
using Gaspra.Logging.Serializer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Gaspra.Logging.Builder
{
    public static class PropertiesHostBuilderExtensions
    {
        public static IServiceCollection AddDefaultLogProperties(
            this IServiceCollection services)
        {
            services.AddSingleton<ILogProperties, DefaultProperties>();

            return services;
        }
    }
}
