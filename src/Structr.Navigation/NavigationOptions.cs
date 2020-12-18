using System;

namespace Structr.Navigation
{
    public class NavigationOptions<TNavigationItem> where TNavigationItem : NavigationItem<TNavigationItem>
    {
        public Type ResourceType { get; set; }
        public Func<TNavigationItem, IServiceProvider, bool> ItemFilter { get; set; }
        public Func<TNavigationItem, IServiceProvider, bool> ItemActivator { get; set; }
        public bool EnableCaching { get; set; }

        public NavigationOptions()
        {
            ItemFilter = (item, serviceProvider) => true;
            ItemActivator = (item, serviceProvider) => false;
            EnableCaching = true;
        }
    }
}
