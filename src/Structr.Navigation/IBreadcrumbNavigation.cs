using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface IBreadcrumbNavigation<out TNavigationItem> : IEnumerable<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        TNavigationItem Active { get; }
    }
}
