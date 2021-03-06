using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigationProvider<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        IEnumerable<TNavigationItem> CreateNavigation();
    }
}
