using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <inheritdoc cref="INavigationCache"/>
    public class NavigationCache : INavigationCache
    {
        private static readonly Type _key = typeof(NavigationCache);
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Initializes an instance of <see cref="NavigationCache"/>.
        /// </summary>
        /// <param name="cache">The <see cref="IMemoryCache"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="cache"/> is <see langword="null"/>.</exception>
        public NavigationCache(IMemoryCache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            _cache = cache;
        }

        public IEnumerable<TNavigationItem> GetOrAdd<TNavigationItem>(Func<IEnumerable<TNavigationItem>> factory)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            var navType = typeof(TNavigationItem);
            var navCache = GetOrCreateCache();
            var nav = navCache.GetOrAdd(navType, t => factory()) as IEnumerable<TNavigationItem>;

            return nav;
        }

        private ConcurrentDictionary<Type, object> GetOrCreateCache()
        {
            ConcurrentDictionary<Type, object> navCache;
            if (_cache.TryGetValue(_key, out object cacheObj))
            {
                navCache = cacheObj as ConcurrentDictionary<Type, object>;
            }
            else
            {
                navCache = new ConcurrentDictionary<Type, object>();
                var cacheOptions = new MemoryCacheEntryOptions { Size = 1 };
                _cache.Set(_key, navCache, cacheOptions);
            }

            return navCache;
        }
    }
}
