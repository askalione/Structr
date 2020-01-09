using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFactory<TKey, TValue>(this IServiceCollection services, Dictionary<TKey, Type> index)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (index == null)
                throw new ArgumentNullException(nameof(index));
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
    }
}
