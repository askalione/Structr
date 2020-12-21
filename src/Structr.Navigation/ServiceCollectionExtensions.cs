using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Navigation;
using Structr.Navigation.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNavigation<TNavigationItem>(this IServiceCollection services,
            INavigationProvider<TNavigationItem> provider,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            services.AddMemoryCache();
            services.TryAddSingleton<INavigationCache, NavigationCache>();
            services.TryAddScoped(typeof(INavigationProvider<TNavigationItem>), serviceProvider => provider);
            services.TryAddScoped(typeof(NavigationOptions<TNavigationItem>), serviceProvider =>
            {
                var options = new NavigationOptions<TNavigationItem>();
                configure?.Invoke(serviceProvider, options);
                return options;
            });
            services.TryAddScoped(typeof(INavigationBuilder<>), typeof(NavigationBuilder<>));
            services.TryAddScoped(typeof(INavigation<>), typeof(Navigation<>));
            services.TryAddScoped(typeof(IBreadcrumbNavigation<>), typeof(BreadcrumbNavigation<>));

            return services;
        }

        public static IServiceCollection AddJsonNavigation<TNavigationItem>(this IServiceCollection services,
            string path,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddNavigation(services, new JsonNavigationProvider<TNavigationItem>(path), configure);

        public static IServiceCollection AddXmlNavigation<TNavigationItem>(this IServiceCollection services,
            string path,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddNavigation(services, new XmlNavigationProvider<TNavigationItem>(path), configure);
    }
}
