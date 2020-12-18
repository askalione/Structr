using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigationBuilder
    {
        IEnumerable<TNavigationItem> BuildNavigation<TNavigationItem>() where TNavigationItem : NavigationItem<TNavigationItem>;
    }
}
