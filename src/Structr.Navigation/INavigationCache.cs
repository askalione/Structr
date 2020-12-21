using System;
using System.Collections.Generic;

namespace Structr.Navigation
{
    public interface INavigationCache
    {
        IEnumerable<TNavigationItem> GetOrAdd<TNavigationItem>(Func<IEnumerable<TNavigationItem>> factory)
             where TNavigationItem : NavigationItem<TNavigationItem>, new();
    }
}
