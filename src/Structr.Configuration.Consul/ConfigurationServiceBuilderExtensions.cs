using Consul;
using Structr.Configuration;
using Structr.Configuration.Consul;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for configuring Configuration services.
    /// </summary>
    public static class ConfigurationServiceBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="ConsulSettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="key">The Consul key.</param>
        /// <param name="consulClient">The <see cref="IConsulClient"/>.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddConsul<TSettings>(this ConfigurationServiceBuilder builder,
            string key,
            IConsulClient consulClient,
            Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
            => AddConsul<TSettings>(builder, key, consulClient, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="ConsulSettingsProvider{TSettings}"/> and related services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TSettings">Settings class.</typeparam>
        /// <param name="builder">The <see cref="ConfigurationServiceBuilder"/>.</param>
        /// <param name="key">The Consul key.</param>
        /// <param name="consulClient">The <see cref="IConsulClient"/>.</param>
        /// <param name="configure">The options object to make additional configurations.</param>
        /// <returns>The <see cref="ConfigurationServiceBuilder"/>.</returns>
        public static ConfigurationServiceBuilder AddConsul<TSettings>(this ConfigurationServiceBuilder builder,
            string key,
            IConsulClient consulClient,
            Action<IServiceProvider, SettingsProviderOptions> configure)
            where TSettings : class, new()
            => builder.AddProvider(serviceProvider =>
            {
                var settingsProviderOptions = new SettingsProviderOptions();
                configure?.Invoke(serviceProvider, settingsProviderOptions);
                var provider = new ConsulSettingsProvider<TSettings>(key,
                    consulClient,
                    settingsProviderOptions);
                return provider;
            });
    }
}
