using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static ConfigurationServiceBuilder AddConfiguration(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = new ConfigurationServiceBuilder(services);

            services.TryAddSingleton(typeof(IConfiguration<>), typeof(Configuration<>));
            services.TryAddSingleton(typeof(IConfigurator<>), typeof(Configurator<>));

            return builder;
        }
    }
}
