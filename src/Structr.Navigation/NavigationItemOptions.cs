using System;

namespace Structr.Navigation
{
    public class NavigationItemOptions<TNavigationItem> where TNavigationItem : NavigationItem<TNavigationItem>
    {
        public Type Resource { get; set; }
        public Func<TNavigationItem, IServiceProvider, bool> Filter { get; set; }
        public Func<TNavigationItem, IServiceProvider, bool> Activator { get; set; }
        public bool Cache { get; set; }

        public NavigationItemOptions()
        {
            Filter = (item, serviceProvider) => true;
            Activator = (item, serviceProvider) => false;
            Cache = true;
        }
    }
}
