using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Represents a type that provides access to a list of instances of <see cref="NavigationItem{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public interface INavigation<out TNavigationItem> : IEnumerable<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Get an active instance of <see cref="NavigationItem{TNavigationItem}"/> from the list.
        /// </summary>
        TNavigationItem Active { get; }
    }
}
