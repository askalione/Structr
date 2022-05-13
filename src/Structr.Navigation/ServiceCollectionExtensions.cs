using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Navigation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring Navigation services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add basic Navigation services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">if <paramref name="services"/> is <see langword="null"/>.</exception>
        public static NavigationServiceBuilder AddNavigation(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = new NavigationServiceBuilder(services);

            services.AddMemoryCache();
            services.TryAddSingleton<INavigationCache, NavigationCache>();
            services.TryAddScoped(typeof(INavigationBuilder<>), typeof(NavigationBuilder<>));
            services.TryAddScoped(typeof(INavigation<>), typeof(Navigation<>));
            services.TryAddScoped(typeof(IBreadcrumbNavigation<>), typeof(BreadcrumbNavigation<>));

            return builder;
        }
    }
}
