using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods.
    /// </summary>
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
    }
}
