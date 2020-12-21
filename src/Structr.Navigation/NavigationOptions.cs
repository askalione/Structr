using System;

namespace Structr.Navigation
{
    public class NavigationOptions<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        public Type ResourceType { get; set; }
        public Func<TNavigationItem, bool> ItemFilter { get; set; }
        public Func<TNavigationItem, bool> ItemActivator { get; set; }
        public bool EnableCaching { get; set; }

        public NavigationOptions()
        {
            ItemFilter = item => true;
            ItemActivator = item => false;
            EnableCaching = true;
        }
    }
}
