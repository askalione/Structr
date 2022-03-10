using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Configuration
{
    public class Configurator<TSettings> : IConfigurator<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions<TSettings> _options;

        public Configurator(IServiceProvider serviceProvider)
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

        public void Configure(Action<TSettings> changes)
        {
            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes));
            }

            var provider = _options.Provider;
            var settings = provider.GetSettings();
            changes.Invoke(settings);
            provider.SetSettings(settings);
        }
    }
}
