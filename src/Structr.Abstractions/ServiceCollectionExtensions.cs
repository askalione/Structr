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
        /// <summary>
        /// Registers service with different implementations determined by provided <typeparamref name="TKey"/>.
        /// This allows to instantiate different implementations of registred service depending on provided key already after the injection.
        /// </summary>
        /// <typeparam name="TKey">Type of key to use.</typeparam>
        /// <typeparam name="TValue">Type of the service to register. All implemintations should be assignable to this type.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="index">Dictionary containing different implementations of declared service <typeparamref name="TValue"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection AddFactory<TKey, TValue>(this IServiceCollection services, IReadOnlyDictionary<TKey, Type> index)
        {
            Ensure.NotNull(services, nameof(services));
            Ensure.NotNull(index, nameof(index));

            if (index.Any() == false)
            {
                throw new ArgumentException(
                    "Items on index not found. At least one item for configured factory is required.");
            }

            var valueType = typeof(TValue);

            foreach (var value in index.Values)
            {
                if (valueType.IsAssignableFrom(value) == false)
                {
                    throw new InvalidOperationException($"Value must be inherited from {valueType}.");
                }

                services.TryAddTransient(value);
            }

            services.AddTransient<Func<TKey, TValue>>(provider =>
            {
                return key =>
                {
                    if (index.TryGetValue(key, out valueType))
                    {
                        return (TValue)provider.GetService(valueType);
                    }
                    else
                    {
                        return default;
                    }
                };
            });

            return services;
        }

        /// <summary>
        /// Adds a singleton timestamp provider service, which implements <see cref="ITimestampProvider"/> interface.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="ITimestampProvider"/></typeparam>
        /// <param name="services"></param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTimestampProvider<T>(this IServiceCollection services) where T : ITimestampProvider
            => AddTimestampProvider<T>(services, ServiceLifetime.Singleton);

        /// <summary>
        /// Adds a timestamp provider service, which implements <see cref="ITimestampProvider"/> interface.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="ITimestampProvider"/></typeparam>
        /// <param name="services"></param>
        /// <param name="lifetime">ServiceLifetime value.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTimestampProvider<T>(this IServiceCollection services, ServiceLifetime lifetime)
            where T : ITimestampProvider
        {
            Ensure.NotNull(services, nameof(services));

            services.TryAdd(new ServiceDescriptor(typeof(ITimestampProvider), typeof(T), lifetime));

            return services;
        }
    }
}
