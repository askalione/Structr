using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring Configuration services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds basic Configuration services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public static ConfigurationServiceBuilder AddConfiguration(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = new ConfigurationServiceBuilder(services);

            services.TryAddSingleton(typeof(IConfiguration<>), typeof(Configuration<>));
            services.TryAddSingleton(typeof(IConfigurator<>), typeof(Configurator<>));

            return builder;
        }
    }
}
