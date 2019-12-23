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
    public static class HostBuilderExtensions
    {
        public static ILoggingBuilder AddProviderFluentd(
            this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .AddFluentd();

            return loggingBuilder;
        }

        public static ILoggingBuilder AddProviderFluentd(
            this ILoggingBuilder loggingBuilder,
            IEnumerable<Type> loggingSerializers)
        {
            loggingBuilder
                .Services
                    .AddSerializers(loggingSerializers);

            loggingBuilder
                .AddFluentd();

            return loggingBuilder;
        }

        public static ILoggingBuilder AddProviderFluentd(this ILoggingBuilder loggingBuilder,
            ILogProperties logProperties,
            IEnumerable<Type> loggingSerializers)
        {
            loggingBuilder
                .Services
                    .AddSingleton(logProperties)
                    .AddSerializers(loggingSerializers);

            loggingBuilder
                .AddFluentd();

            return loggingBuilder;
        }

        public static ILoggingBuilder AddProviderFluentdWithDefaults(
            this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder
                .Services
                    .AddDefaultLogProperties()
                    .AddDefaultLogSerializer();

            loggingBuilder
                .AddFluentd();

            return loggingBuilder;
        }

        public static ILoggingBuilder AddProviderFluentdWithDefaults(
            this ILoggingBuilder loggingBuilder,
            FluentdOptions fluentdOptions)
        {
            loggingBuilder
                .Services
                    .AddDefaultLogProperties()
                    .AddDefaultLogSerializer();

            loggingBuilder
                .AddFluentd(fluentdOptions);

            return loggingBuilder;
        }

        public static IServiceCollection AddDefaultLogProperties(
            this IServiceCollection services)
        {
            services.AddSingleton<ILogProperties, DefaultProperties>();

            return services;
        }

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
