using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Navigation;
using Structr.Navigation.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NavigationServiceBuilderExtensions
    {
        public static NavigationServiceBuilder AddProvider<TNavigationItem>(this NavigationServiceBuilder builder,
            INavigationProvider<TNavigationItem> provider,
            Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddProvider(builder, provider, (_, options) => configure?.Invoke(options));

        public static NavigationServiceBuilder AddProvider<TNavigationItem>(this NavigationServiceBuilder builder,
            INavigationProvider<TNavigationItem> provider,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
        {
            builder.Services.TryAddScoped(typeof(INavigationProvider<TNavigationItem>), serviceProvider => provider);
            builder.Services.TryAddScoped(typeof(NavigationOptions<TNavigationItem>), serviceProvider =>
            {
                var options = new NavigationOptions<TNavigationItem>();
                configure?.Invoke(serviceProvider, options);
                return options;
            });

            return builder;
        }

        public static NavigationServiceBuilder AddJson<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddJson<TNavigationItem>(builder, path, (_, options) => configure?.Invoke(options));

        public static NavigationServiceBuilder AddJson<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddProvider(builder, new JsonNavigationProvider<TNavigationItem>(path), configure);

        public static NavigationServiceBuilder AddXml<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddXml<TNavigationItem>(builder, path, (_, options) => configure?.Invoke(options));

        public static NavigationServiceBuilder AddXml<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddProvider(builder, new XmlNavigationProvider<TNavigationItem>(path), configure);
    }
}
