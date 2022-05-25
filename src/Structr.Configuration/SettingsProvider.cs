using Structr.Configuration.Internal;
using System;

namespace Structr.Configuration
{
    public abstract class SettingsProvider<TSettings> where TSettings : class, new()
    {
        private readonly SettingsProviderOptions _options;
        private TSettings _cache;

        public SettingsProvider(SettingsProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public TSettings GetSettings()
        {
            if (_options.Cache == false || _cache == null)
            {
                _cache = LoadSettings();
            }
            else
            {
                var isModified = IsSettingsModified();
                if (isModified)
                {
                    var settings = LoadSettings();
                    Mapper.Map(settings, _cache);
                }
            }

            return _cache;
        }

        protected abstract TSettings LoadSettings();

        public void SetSettings(TSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            UpdateSettings(settings);
        }

        protected abstract void UpdateSettings(TSettings settings);

        protected abstract bool IsSettingsModified();
    }
}
