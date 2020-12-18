using Structr.Navigation.Providers;
using System;

namespace Structr.Navigation
{
    public static class NavigationConfiguratorExtensions
    {
        public static NavigationConfigurator AddJson<TNavigationItem>(this NavigationConfigurator configurator,
            string path,
            Action<NavigationOptions<TNavigationItem>> configure = null) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (configurator == null)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            configurator.Add(new JsonNavigationProvider<TNavigationItem>(path), configure);

            return configurator;
        }
    }
}
