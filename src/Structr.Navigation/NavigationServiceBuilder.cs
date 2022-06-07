using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Navigation
{
    /// <summary>
    /// An instance for configuring Navigation services.
    /// </summary>
    public class NavigationServiceBuilder
    {
        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> where Navigation services are configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Initializes a new <see cref="NavigationServiceBuilder"/> instance.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public NavigationServiceBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
        }
    }
}
