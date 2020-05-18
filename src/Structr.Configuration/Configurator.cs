using System;

namespace Structr.Configuration
{
    public class Configurator<TSettings> : IConfigurator<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions _options;
        private readonly IConfiguration<TSettings> _configuration;

        public Configurator(ConfigurationOptions options,
            IConfiguration<TSettings> configuration)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _options = options;
            _configuration = configuration;
        }

        public void Configure(Action<TSettings> changes)
        {
            if (changes == null)
                throw new ArgumentNullException(nameof(changes));

            var provider = _options.Providers.Get<TSettings>();
            var settings = _configuration.Settings;
            changes.Invoke(settings);
            provider.SetSettings(settings);
        }
    }
}
