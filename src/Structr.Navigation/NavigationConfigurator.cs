using System;
using System.Collections.Concurrent;

namespace Structr.Navigation
{
    public class NavigationConfigurator
    {
        private static readonly ConcurrentDictionary<Type, object> _configurations = new ConcurrentDictionary<Type, object>();

        public NavigationConfigurator() { }

        public void Add<TNavigationItem>(INavigationProvider<TNavigationItem> provider, Action<NavigationOptions<TNavigationItem>> configure = null)
            where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            var navType = typeof(TNavigationItem);

            _configurations.GetOrAdd(navType, t =>
            {
                var options = new NavigationOptions<TNavigationItem>();
                configure?.Invoke(options);

                var configuration = new NavigationConfiguration<TNavigationItem>(provider, options);
                return configuration;
            });
        }

        internal NavigationConfiguration<TNavigationItem> Get<TNavigationItem>()
            where TNavigationItem : NavigationItem<TNavigationItem>
        {
            var navType = typeof(TNavigationItem);

            if (_configurations.TryGetValue(navType, out object configuration))
            {
                return configuration as NavigationConfiguration<TNavigationItem>;
            }

            return null;
        }
    }
}
