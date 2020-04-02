using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigationBuilder
    {
        IEnumerable<TNavigationItem> Build<TNavigationItem>() where TNavigationItem : NavigationItem<TNavigationItem>;
    }
}
