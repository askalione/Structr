using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigationBuilder<TNavigationItem>
         where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        IEnumerable<TNavigationItem> BuildNavigation();
    }
}
