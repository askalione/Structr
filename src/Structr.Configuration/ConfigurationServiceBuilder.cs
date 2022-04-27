using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Configuration
{
    public class ConfigurationServiceBuilder
    {
        public IServiceCollection Services { get; }

        public ConfigurationServiceBuilder(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            Services = services;
        }
    }
}
