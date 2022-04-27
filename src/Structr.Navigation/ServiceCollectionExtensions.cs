using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Navigation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
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
