using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gaspra.Logging.Serializer.Extensions
{
    public static class SerializerExtension
    {
        /*
            Add serializer extensions
        */
        public static IServiceCollection AddSerializersFromAssemblies(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            var typesFromAssemblies = assemblies
                .SelectMany(a => a
                    .DefinedTypes
                    .Where(t => t
                        .GetInterfaces()
                        .Contains(typeof(ILogSerializer))));

            foreach (var type in typesFromAssemblies)
            {
                services.Add(
                    new ServiceDescriptor(
                        typeof(ILogSerializer),
                        type,
                        lifetime));
            }

            return services;
        }

        public static IServiceCollection AddSerializers(
            this IServiceCollection services,
            IEnumerable<Type> serializers,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            foreach (var serializer in serializers)
            {
                if (serializer.GetInterface(typeof(ILogSerializer).Name) == null)
                {
                    throw new ArgumentException($"{serializer.GetType().Name} doesn't implement {typeof(ILogSerializer).Name}, unable to add to services.", nameof(serializers));
                }

                services.Add(new ServiceDescriptor(
                    typeof(ILogSerializer),
                    serializer,
                    lifetime));
            }

            return services;
        }

        public static IServiceCollection TryAddSerializer(
            this IServiceCollection services,
            Type serializer,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            services
                .TryAdd(new ServiceDescriptor(
                    typeof(ILogSerializer),
                    serializer,
                    lifetime));

            return services;
        }

        /*
            Get serializer extensions
        */
        public static IEnumerable<ILogSerializer> GetSerializers(
            this IServiceCollection collection)
        {
            var provider = collection.BuildServiceProvider();

            return provider.GetSerializers();
        }

        public static IEnumerable<ILogSerializer> GetSerializers(
            this IServiceProvider provider)
        {
            return provider.GetSerializers(p => nameof(p));
        }

        public static IEnumerable<ILogSerializer> GetSerializers<TKey>(
            this IServiceCollection collection,
            Func<ILogSerializer, TKey> keySelector)
        {
            var provider = collection.BuildServiceProvider();

            return provider.GetSerializers(keySelector);
        }

        public static IEnumerable<ILogSerializer> GetSerializers<TKey>(
            this IServiceProvider provider,
            Func<ILogSerializer, TKey> keySelector)
        {
            var providers = provider
                .GetServices<ILogSerializer>()
                .ToList()
                .OrderBy(keySelector);

            return providers;
        }

        public static IEnumerable<ILogSerializer> GetSerializersDescending<TKey>(
            this IServiceCollection collection,
            Func<ILogSerializer, TKey> keySelector)
        {
            var provider = collection.BuildServiceProvider();

            return provider.GetSerializersDescending(keySelector);
        }

        public static IEnumerable<ILogSerializer> GetSerializersDescending<TKey>(
            this IServiceProvider provider,
            Func<ILogSerializer, TKey> keySelector)
        {
            var providers = provider
                .GetServices<ILogSerializer>()
                .ToList()
                .OrderByDescending(keySelector);

            return providers;
        }
    }
}
