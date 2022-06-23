using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Configuration
{
    /// <summary>
    /// An instance for configuring Configuration services.
    /// </summary>
    public class ConfigurationServiceBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where Configuration services are configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new <see cref="ConfigurationServiceBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public ConfigurationServiceBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
        }
    }
}
