using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Navigation;
using Structr.Navigation.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring Navigation services.
    /// </summary>
    public static class NavigationServiceBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="INavigationProvider{TNavigationItem}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="builder">The <see cref="NavigationServiceBuilder"/>.</param>
        /// <param name="provider">The <see cref="INavigationProvider{TNavigationItem}"/>. For example, the <see cref="JsonNavigationProvider{TNavigationItem}"/> or the <see cref="XmlNavigationProvider{TNavigationItem}"/>.</param>
        /// <param name="configure">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
        public static NavigationServiceBuilder AddProvider<TNavigationItem>(this NavigationServiceBuilder builder,
            INavigationProvider<TNavigationItem> provider,
            Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddProvider(builder, provider, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="INavigationProvider{TNavigationItem}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="builder">The <see cref="NavigationServiceBuilder"/>.</param>
        /// <param name="provider">The <see cref="INavigationProvider{TNavigationItem}"/>. For example, the <see cref="JsonNavigationProvider{TNavigationItem}"/> or the <see cref="XmlNavigationProvider{TNavigationItem}"/>.</param>
        /// <param name="configure">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
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

        /// <summary>
        /// Adds <see cref="JsonNavigationProvider{TNavigationItem}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="builder">The <see cref="NavigationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to <strong>JSON</strong> file with navigation configuration.</param>
        /// <param name="configure">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
        public static NavigationServiceBuilder AddJson<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddJson<TNavigationItem>(builder, path, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="JsonNavigationProvider{TNavigationItem}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="builder">The <see cref="NavigationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to <strong>JSON</strong> file with navigation configuration.</param>
        /// <param name="configure">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
        public static NavigationServiceBuilder AddJson<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddProvider(builder, new JsonNavigationProvider<TNavigationItem>(path), configure);

        /// <summary>
        /// Adds <see cref="XmlNavigationProvider{TNavigationItem}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="builder">The <see cref="NavigationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to <strong>XML</strong> file with navigation configuration.</param>
        /// <param name="configure">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
        public static NavigationServiceBuilder AddXml<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddXml<TNavigationItem>(builder, path, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="XmlNavigationProvider{TNavigationItem}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="builder">The <see cref="NavigationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to <strong>XML</strong> file with navigation configuration.</param>
        /// <param name="configure">The <see cref="NavigationOptions{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="NavigationServiceBuilder"/>.</returns>
        public static NavigationServiceBuilder AddXml<TNavigationItem>(this NavigationServiceBuilder builder,
            string path,
            Action<IServiceProvider, NavigationOptions<TNavigationItem>> configure)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
            => AddProvider(builder, new XmlNavigationProvider<TNavigationItem>(path), configure);
    }
}
