using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigation<out TNavigationItem> : IEnumerable<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        TNavigationItem Active { get; }
    }
}
