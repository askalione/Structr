using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Abstractions;
using Structr.Abstractions.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFactory<TKey, TValue>(this IServiceCollection services, Dictionary<TKey, Type> index)
        {
            Ensure.NotNull(services, nameof(services));
            Ensure.NotNull(index, nameof(index));

            if (!index.Any())
                throw new ArgumentException("Not found items on index. At least one item for configure factory is required");

            var valueType = typeof(TValue);

            foreach (var value in index.Values)
            {
                if (!valueType.IsAssignableFrom(value))
                    throw new InvalidOperationException($"Value must be inherited from {valueType}");

                services.TryAddTransient(value);
            }

            services.AddTransient<Func<TKey, TValue>>(provider =>
            {
                return key =>
                {
                    if (index.TryGetValue(key, out valueType))
                        return (TValue)provider.GetService(valueType);
                    else
                        return default(TValue);
                };
            });

            return services;
        }

        public static IServiceCollection AddTimestampProvider<T>(this IServiceCollection services) where T : ITimestampProvider
            => AddTimestampProvider<T>(services, ServiceLifetime.Singleton);

        public static IServiceCollection AddTimestampProvider<T>(this IServiceCollection services, ServiceLifetime lifetime)
            where T : ITimestampProvider
        {
            Ensure.NotNull(services, nameof(services));

            services.TryAdd(new ServiceDescriptor(typeof(ITimestampProvider), typeof(T), lifetime));

            return services;
        }
    }
}
