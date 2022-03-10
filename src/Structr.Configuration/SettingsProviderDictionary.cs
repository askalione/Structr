//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace Structr.Configuration
//{
//    public class SettingsProviderDictionary : IReadOnlyDictionary<Type, object>
//    {
//        private readonly Dictionary<Type, object> _providers = new Dictionary<Type, object>();

//        public object this[Type key] => _providers[key];

//        public IEnumerable<Type> Keys => _providers.Keys;

//        public IEnumerable<object> Values => _providers.Values;

//        public int Count => _providers.Count;

//        public SettingsProviderDictionary() { }

//        public bool ContainsKey(Type key) => _providers.ContainsKey(key);

//        public bool TryGetValue(Type key, out object value) => _providers.TryGetValue(key, out value);

//        public IEnumerator<KeyValuePair<Type, object>> GetEnumerator() => _providers.GetEnumerator();

//        IEnumerator IEnumerable.GetEnumerator()
//             => GetEnumerator();

//        public void Add<TSettings>(SettingsProvider<TSettings> provider) where TSettings : class, new()
//        {
//            if (provider == null)
//                throw new ArgumentNullException(nameof(provider));

//            var type = typeof(TSettings);

//            _providers[type] = provider;
//        }

//        public bool TryGet<TSettings>(out SettingsProvider<TSettings> provider) where TSettings : class, new()
//        {
//            provider = null;
//            var type = typeof(TSettings);
//            if (_providers.ContainsKey(type))
//            {
//                provider = (SettingsProvider<TSettings>)_providers[type];
//                return true;
//            }

//            return false;
//        }

//        public SettingsProvider<TSettings> Get<TSettings>() where TSettings : class, new()
//        {
//            var type = typeof(TSettings);
//            if (_providers.TryGetValue(type, out object obj))
//            {
//                var provider = (SettingsProvider<TSettings>)obj;
//                return provider;
//            }

//            throw new InvalidOperationException($"Settings provider for type {type.Name} not configured."); ;
//        }
//    }
//}
