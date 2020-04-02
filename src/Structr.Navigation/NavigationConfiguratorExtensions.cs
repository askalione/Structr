using Structr.Navigation.Providers;
using System;

namespace Structr.Navigation
{
    public static class NavigationConfiguratorExtensions
    {
        public static NavigationConfigurator AddJson<TNavigationItem>(this NavigationConfigurator configurations,
            string path,
            Action<NavigationItemOptions<TNavigationItem>> configure = null) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (configurations == null)
                throw new ArgumentNullException(nameof(configurations));

            configurations.Add(new JsonNavigationProvider(path), configure);

            return configurations;
        }

        public static NavigationConfigurator AddXml<TNavigationItem>(this NavigationConfigurator configurations,
            string path,
            Action<NavigationItemOptions<TNavigationItem>> configure = null) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (configurations == null)
                throw new ArgumentNullException(nameof(configurations));

            configurations.Add(new XmlNavigationProvider(path), configure);

            return configurations;
        }
    }
}
