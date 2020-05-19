using System;

namespace Structr.Configuration
{
    public class Configuration<TSettings> : IConfiguration<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions _options;

        public TSettings Settings
            => _options.Providers.Get<TSettings>().GetSettings();

        public Configuration(ConfigurationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }
    }
}
