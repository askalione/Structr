using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Navigation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNavigation(this IServiceCollection services, Action<NavigationConfigurator> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var configurator = new NavigationConfigurator(services);
            configure.Invoke(configurator);

            services.AddMemoryCache();
            services.TryAddSingleton<INavigationBuilder, NavigationBuilder>();
            services.TryAddScoped(typeof(IMenuNavigation<>), typeof(MenuNavigation<>));
            services.TryAddScoped(typeof(IBreadcrumbNavigation<>), typeof(BreadcrumbNavigation<>));

            return services;
        }
    }
}
