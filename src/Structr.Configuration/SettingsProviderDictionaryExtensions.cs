using Structr.Configuration.Providers;
using System;

namespace Structr.Configuration
{
    public static class SettingsProviderDictionaryExtensions
    {
        public static SettingsProviderDictionary AddJson<TSettings>(this SettingsProviderDictionary providers, string path)
            where TSettings : class, new()
        {
            if (providers == null)
                throw new ArgumentNullException(nameof(providers));

            var provider = new JsonSettingsProvider<TSettings>(path);
            providers.Add(provider);

            return providers;
        }

        public static SettingsProviderDictionary AddXml<TSettings>(this SettingsProviderDictionary providers, string path)
            where TSettings : class, new()
        {
            if (providers == null)
                throw new ArgumentNullException(nameof(providers));

            var provider = new XmlSettingsProvider<TSettings>(path);
            providers.Add(provider);

            return providers;
        }
    }
}
