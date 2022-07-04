using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Abstractions;
using Structr.Abstractions.Providers.Timestamp;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring timestamp services.
    /// </summary>
    public static class TimestampServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a singleton timestamp provider service, which implements <see cref="ITimestampProvider"/> interface.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="ITimestampProvider"/></typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTimestampProvider<T>(this IServiceCollection services) where T : ITimestampProvider
            => AddTimestampProvider<T>(services, ServiceLifetime.Singleton);

        /// <summary>
        /// Adds a timestamp provider service, which implements <see cref="ITimestampProvider"/> interface.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="ITimestampProvider"/></typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
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
