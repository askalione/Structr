using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration<TSettings>(this IServiceCollection services, SettingsProvider<TSettings> provider)
            where TSettings : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            services.AddSingleton(serviceProvider =>
            {
                var options = new ConfigurationOptions<TSettings>(provider);
                return options;
            });

            services.TryAddSingleton(typeof(IConfiguration<>), typeof(Configuration<>));
            services.TryAddSingleton(typeof(IConfigurator<>), typeof(Configurator<>));

            return services;
        }

        public static IServiceCollection AddJsonConfiguration<TSettings>(this IServiceCollection services, string path, Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var options = new SettingsProviderOptions();
            configure?.Invoke(options);
            var provider = new JsonSettingsProvider<TSettings>(options, path);

            services.AddConfiguration(provider);

            return services;
        }

        public static IServiceCollection AddXmlConfiguration<TSettings>(this IServiceCollection services, string path, Action<SettingsProviderOptions> configure = null)
            where TSettings : class, new()
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var options = new SettingsProviderOptions();
            configure?.Invoke(options);
            var provider = new XmlSettingsProvider<TSettings>(options, path);

            services.AddConfiguration(provider);

            return services;
        }

        //public static IServiceCollection AddConfiguration(this IServiceCollection services, Action<ConfigurationOptions> configure)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (configure == null)
        //        throw new ArgumentNullException(nameof(configure));

        //    AddConfiguration(services, (serviceProvider, options) =>
        //    {
        //        configure.Invoke(options);
        //    });

        //    return services;
        //}

        //public static IServiceCollection AddConfiguration(this IServiceCollection services, Action<IServiceProvider, ConfigurationOptions> configure)
        //{
        //    if (services == null)
        //        throw new ArgumentNullException(nameof(services));
        //    if (configure == null)
        //        throw new ArgumentNullException(nameof(configure));

        //    services.TryAddSingleton(serviceProvider =>
        //    {
        //        var options = new ConfigurationOptions();
        //        configure.Invoke(serviceProvider, options);
        //        return options;
        //    });
        //    services.TryAddSingleton(typeof(IConfiguration<>), typeof(Configuration<>));
        //    services.TryAddSingleton(typeof(IConfigurator<>), typeof(Configurator<>));

        //    return services;
        //}
    }
}
