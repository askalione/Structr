using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Represents a type that can build list of instances of <see cref="NavigationItem{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public interface INavigationBuilder<TNavigationItem>
         where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Build list of instances of <see cref="NavigationItem{TNavigationItem}"/>.
        /// </summary>
        IEnumerable<TNavigationItem> BuildNavigation();
    }
}
