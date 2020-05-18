using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services, Action<ConfigurationOptions> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            AddConfiguration(services, (serviceProvider, options) =>
            {
                configure.Invoke(options);
            });

            return services;
        }

        public static IServiceCollection AddConfiguration(this IServiceCollection services, Action<IServiceProvider, ConfigurationOptions> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            services.TryAddSingleton(serviceProvider =>
            {
                var options = new ConfigurationOptions();
                configure.Invoke(serviceProvider, options);
                return options;
            });
            services.TryAddSingleton(typeof(IConfiguration<>), typeof(Configuration<>));
            services.TryAddSingleton(typeof(IConfigurator<>), typeof(Configurator<>));

            return services;
        }
    }
}
