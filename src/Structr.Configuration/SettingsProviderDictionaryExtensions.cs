//using Structr.Configuration.Providers;
//using System;

//namespace Structr.Configuration
//{
//    public static class SettingsProviderDictionaryExtensions
//    {
//        public static SettingsProviderDictionary AddJson<TSettings>(this SettingsProviderDictionary providers, string path, Action<SettingsProviderOptions> configure = null)
//            where TSettings : class, new()
//        {
//            if (providers == null)
//                throw new ArgumentNullException(nameof(providers));

//            var options = new SettingsProviderOptions();
//            configure?.Invoke(options);

//            var provider = new JsonSettingsProvider<TSettings>(options, path);
//            providers.Add(provider);

//            return providers;
//        }

//        public static SettingsProviderDictionary AddXml<TSettings>(this SettingsProviderDictionary providers, string path, Action<SettingsProviderOptions> configure = null)
//            where TSettings : class, new()
//        {
//            if (providers == null)
//                throw new ArgumentNullException(nameof(providers));

//            var options = new SettingsProviderOptions();
//            configure?.Invoke(options);

//            var provider = new XmlSettingsProvider<TSettings>(options, path);
//            providers.Add(provider);

//            return providers;
//        }
//    }
//}
