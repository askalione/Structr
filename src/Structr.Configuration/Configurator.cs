using System;

namespace Structr.Configuration
{
    public class Configurator<TSettings> : IConfigurator<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions _options;

        public Configurator(ConfigurationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        public void Configure(Action<TSettings> changes)
        {
            if (changes == null)
                throw new ArgumentNullException(nameof(changes));

            var provider = _options.Providers.Get<TSettings>();
            var settings = provider.GetSettings();
            changes.Invoke(settings);
            provider.SetSettings(settings);
        }
    }
}
