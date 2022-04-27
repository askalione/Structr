using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationServiceBuilderExtensions
    {
        public static ConfigurationServiceBuilder AddProvider<TSettings>(this ConfigurationServiceBuilder builder,
            SettingsProvider<TSettings> provider)
            where TSettings : class, new()
            => AddProvider(builder, _ => provider);

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

        public static ConfigurationServiceBuilder AddJson<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
            => AddJson<TSettings>(builder, path, (_, options) => configure?.Invoke(options));

        public static ConfigurationServiceBuilder AddJson<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<IServiceProvider, SettingsProviderOptions> configure)
            where TSettings : class, new()
            => AddProvider(builder, serviceProvider =>
            {
                var settingsProviderOptions = new SettingsProviderOptions();
                configure?.Invoke(serviceProvider, settingsProviderOptions);
                var provider = new JsonSettingsProvider<TSettings>(settingsProviderOptions, path);
                return provider;
            });

        public static ConfigurationServiceBuilder AddXml<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
            => AddXml<TSettings>(builder, path, (_, options) => configure?.Invoke(options));

        public static ConfigurationServiceBuilder AddXml<TSettings>(this ConfigurationServiceBuilder builder,
            string path,
            Action<IServiceProvider, SettingsProviderOptions> configure)
            where TSettings : class, new()
            => AddProvider(builder, serviceProvider =>
            {
                var settingsProviderOptions = new SettingsProviderOptions();
                configure?.Invoke(serviceProvider, settingsProviderOptions);
                var provider = new XmlSettingsProvider<TSettings>(settingsProviderOptions, path);
                return provider;
            });
    }
}
