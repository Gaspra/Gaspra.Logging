using Gaspra.Logging.Serializer;
using Gaspra.Logging.Serializer.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Gaspra.Logging.Builder
{
    public static class SerializerHostBuilderExtensions
    {
        public static IServiceCollection AddDefaultLogSerializer(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services
                .AddSerializers(
                    new Type[] { typeof(DefaultSerializer) },
                    lifetime);

            return services;
        }
    }
}
