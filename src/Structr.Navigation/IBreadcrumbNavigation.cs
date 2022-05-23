using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Provides functionality for access to a list of breadcrumb navigation items <see cref="IEnumerable{TNavigationItem}"/>.
    /// </summary>
    /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
    public interface IBreadcrumbNavigation<out TNavigationItem> : IEnumerable<TNavigationItem>
        where TNavigationItem : NavigationItem<TNavigationItem>, new()
    {
        /// <summary>
        /// Returns an active breadcrumb item <see cref="TNavigationItem"/>.
        /// </summary>
        TNavigationItem Active { get; }
    }
}
