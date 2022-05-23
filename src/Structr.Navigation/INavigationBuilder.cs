using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Provides functionality for build a list of navigation items <see cref="IEnumerable{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public interface INavigationBuilder<TNavigationItem>
         where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Build list of navigation items.
        /// </summary>
        IEnumerable<TNavigationItem> BuildNavigation();
    }
}
