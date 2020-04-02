using Structr.Navigation.Providers;
using System;

namespace Structr.Navigation
{
    public static class NavigationConfiguratorExtensions
    {
        public static NavigationConfigurator AddJson<TNavigationItem>(this NavigationConfigurator configurations,
            string path,
            NavigationItemOptions<TNavigationItem> options = null) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (configurations == null)
                throw new ArgumentNullException(nameof(configurations));

            configurations.Add(new JsonNavigationProvider(path), options);

            return configurations;
        }

        public static NavigationConfigurator AddXml<TNavigationItem>(this NavigationConfigurator configurations,
            string path,
            NavigationItemOptions<TNavigationItem> options = null) where TNavigationItem : NavigationItem<TNavigationItem>
        {
            if (configurations == null)
                throw new ArgumentNullException(nameof(configurations));

            configurations.Add(new XmlNavigationProvider(path), options);

            return configurations;
        }
    }
}
