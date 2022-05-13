using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Represents a type that can create list of instances of <see cref="NavigationItem{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public interface INavigationProvider<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Create list of instances of <see cref="NavigationItem{TNavigationItem}"/>.
        /// </summary>
        IEnumerable<TNavigationItem> CreateNavigation();
    }
}
