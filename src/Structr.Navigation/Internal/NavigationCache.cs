using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;

namespace Structr.Navigation.Internal
{
    internal static class NavigationCache
    {
        private static readonly Type _key = typeof(NavigationCache);

        private static ConcurrentDictionary<Type, JArray> GetOrCreateCache(IMemoryCache cache)
        {
            ConcurrentDictionary<Type, JArray> navCache;
            if (cache.TryGetValue(_key, out object cacheObj))
            {
                navCache = cacheObj as ConcurrentDictionary<Type, JArray>;
            }
            else
            {
                navCache = new ConcurrentDictionary<Type, JArray>();
                var cacheOptions = new MemoryCacheEntryOptions { Size = 1 };
                cache.Set(_key, navCache, cacheOptions);
            }

            return navCache;
        }

        internal static JArray GetOrAdd<TNavigationItem>(IMemoryCache cache, Func<JArray> factory) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));

            var navCache = GetOrCreateCache(cache);
            var navType = typeof(TNavigationItem);
            JArray jNav;

            if (navCache.ContainsKey(navType))
            {
                jNav = navCache[navType];
            }
            else
            {
                jNav = factory();
                navCache.GetOrAdd(navType, jNav);
            }

            return jNav;
        }
    }
}
