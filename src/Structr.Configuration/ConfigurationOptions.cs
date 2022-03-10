using System;

namespace Structr.Configuration
{
    public class ConfigurationOptions<TSettings> where TSettings : class, new()
    {
        public SettingsProvider<TSettings> Provider { get; }

        public ConfigurationOptions(SettingsProvider<TSettings> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            Provider = provider;
        }
    }
}
