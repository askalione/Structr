using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Configuration
{
    public class Configuration<TSettings> : IConfiguration<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions<TSettings> _options;

        public TSettings Settings => _options.Provider.GetSettings();

        public Configuration(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var options = serviceProvider.GetService<ConfigurationOptions<TSettings>>();
            if (options == null)
            {
                throw new InvalidOperationException($"Settings of type \"{typeof(TSettings).Name}\" not configured.");
            }

            _options = options;
        }
    }
}
