using System;

namespace Structr.Navigation
{
    public class NavigationConfiguration<TNavigationItem> where TNavigationItem : NavigationItem<TNavigationItem>
    {
        public Type Type => typeof(TNavigationItem);
        public INavigationProvider Provider { get; }
        public NavigationItemOptions<TNavigationItem> Options { get; }

        public NavigationConfiguration(INavigationProvider provider, NavigationItemOptions<TNavigationItem> options)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            Provider = provider;
            Options = options;
        }
    }
}
