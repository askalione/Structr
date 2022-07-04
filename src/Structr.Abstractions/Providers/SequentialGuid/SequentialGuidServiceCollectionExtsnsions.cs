using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Abstractions;
using Structr.Abstractions.Providers.SequentialGuid;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring sequential GUID services.
    /// </summary>
    public static class SequentialGuidServiceCollectionExtsnsions
    {
        /// <summary>
        /// Adds a singleton sequential GUID provider with default initializer and unique timestamp provider.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSequentialGuidProvider(this IServiceCollection services)
            => AddSequentialGuidProvider(services, Guid.NewGuid);

        /// <summary>
        /// Adds a singleton sequential GUID provider with specified <paramref name="initializer"/> and unique timestamp provider.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="initializer">GUID initializer.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSequentialGuidProvider(this IServiceCollection services,
            SequentialGuidInitializer initializer)
            => AddSequentialGuidProvider(services, initializer, new UniqueSequentialGuidTimestampProvider().GetTimestamp);

        /// <summary>
        /// Adds a singleton sequential GUID provider with specified <paramref name="initializer"/> and <paramref name="timestampProvider"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="initializer">GUID initializer.</param>
        /// <param name="timestampProvider">The Timestamp provider.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddSequentialGuidProvider(this IServiceCollection services,
            SequentialGuidInitializer initializer,
            SequentialGuidTimestampProvider timestampProvider)
        {
            Ensure.NotNull(services, nameof(services));
            Ensure.NotNull(initializer, nameof(initializer));
            Ensure.NotNull(timestampProvider, nameof(timestampProvider));

            services.TryAddSingleton<ISequentialGuidProvider>(_ => new SequentialGuidProvider(initializer, timestampProvider));

            return services;
        }
    }
}
