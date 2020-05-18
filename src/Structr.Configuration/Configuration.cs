using Structr.Configuration.Internal;
using System;

namespace Structr.Configuration
{
    public class Configuration<TSettings> : IConfiguration<TSettings> where TSettings : class, new()
    {
        private readonly ConfigurationOptions _options;

        private TSettings _settings = null;
        public TSettings Settings
        {
            get
            {
                var provider = _options.Providers.Get<TSettings>();

                if (_options.Cache == false || _settings == null)
                {
                    _settings = provider.GetSettings();
                }
                else
                {
                    if (provider.IsSettingsChanged())
                    {
                        var settings = provider.GetSettings();
                        Mapper.Map(settings, _settings);
                    }
                }

                return _settings;
            }
        }

        public Configuration(ConfigurationOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }
    }
}
