using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Provides functionality for creation list of navigation items <see cref="IEnumerable{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public interface INavigationProvider<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Create list of navigation items <see cref="NavigationItem{TNavigationItem}"/>.
        /// </summary>
        IEnumerable<TNavigationItem> CreateNavigation();
    }
}
