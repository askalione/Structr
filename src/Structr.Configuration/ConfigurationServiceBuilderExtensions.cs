using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring Configuration services.
    /// </summary>
    public static class ConfigurationServiceBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="SettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="provider">The provider to be used for accessing to settings.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddProvider<TSettings>(this ConfigurationServiceBuilder builder,
            SettingsProvider<TSettings> provider)
            where TSettings : class, new()
            => AddProvider(builder, _ => provider);

        /// <summary>
        /// Adds <see cref="SettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="providerFactory">Delegate that takes service provider <see cref="IServiceProvider"/> and returns a settings provider <see cref="SettingsProvider{TNavigationItem}"/>.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="providerFactory"/> is <see langword="null"/>.</exception>
        public static ConfigurationServiceBuilder AddProvider<TSettings>(this ConfigurationServiceBuilder builder,
            Func<IServiceProvider, SettingsProvider<TSettings>> providerFactory)
            where TSettings : class, new()
        {
            if (providerFactory == null)
            {
                throw new ArgumentNullException(nameof(providerFactory));
            }

            builder.Services.TryAddSingleton(serviceProvider =>
            {
                var configurationOptions = new ConfigurationOptions<TSettings>(providerFactory(serviceProvider));

                return configurationOptions;
            });

            return builder;
        }

        /// <summary>
        /// Adds <see cref="JsonSettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to JSON file with settings.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddJson<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
            => AddJson<TSettings>(builder, path, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="JsonSettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to JSON file with settings.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddJson<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<IServiceProvider, SettingsProviderOptions> configure)
            where TSettings : class, new()
            => AddProvider(builder, serviceProvider =>
            {
                var settingsProviderOptions = new SettingsProviderOptions();
                configure?.Invoke(serviceProvider, settingsProviderOptions);
                var provider = new JsonSettingsProvider<TSettings>(path, settingsProviderOptions);
                return provider;
            });

        /// <summary>
        /// Adds <see cref="XmlSettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to XML file with settings.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddXml<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
            => AddXml<TSettings>(builder, path, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="XmlSettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to XML file with settings.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddXml<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<IServiceProvider, SettingsProviderOptions> configure)
            where TSettings : class, new()
            => AddProvider(builder, serviceProvider =>
            {
                var settingsProviderOptions = new SettingsProviderOptions();
                configure?.Invoke(serviceProvider, settingsProviderOptions);
                var provider = new XmlSettingsProvider<TSettings>(path, settingsProviderOptions);
                return provider;
            });

        /// <summary>
        /// Adds <see cref="InMemorySettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings type.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="settings">Settings class.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddInMemory<TSettings>(this ConfigurationServiceBuilder builder,
            TSettings settings,
            Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
            => AddInMemory(builder, settings, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="InMemorySettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings type.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="settings">Settings class.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddInMemory<TSettings>(this ConfigurationServiceBuilder builder,
            TSettings settings,
            Action<IServiceProvider, SettingsProviderOptions> configure)
            where TSettings : class, new()
            => AddProvider(builder, serviceProvider =>
            {
                var settingsProviderOptions = new SettingsProviderOptions();
                configure?.Invoke(serviceProvider, settingsProviderOptions);
                var provider = new InMemorySettingsProvider<TSettings>(settings, settingsProviderOptions);
                return provider;
            });
    }
}
