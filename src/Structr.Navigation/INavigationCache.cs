using System;
using System.Collections.Generic;

namespace Structr.Navigation
{
    /// <summary>
    /// Provides functionality for implements a navigation cache.
    /// </summary>
    public interface INavigationCache
    {
        /// <summary>
        /// Adds a list of instances of <see cref="NavigationItem{TNavigationItem}"/> to the cache if cache does not already contain the list.
        /// Returns the list of instances of <see cref="NavigationItem{TNavigationItem}"/> from cache.
        /// </summary>
        /// <typeparam name="TNavigationItem">An implementation of the abstract class <see cref="NavigationItem{TNavigationItem}"/>.</typeparam>
        /// <param name="factory">Delegate that returns a list of instances of <see cref="NavigationItem{TNavigationItem}"/>.</param>
        /// <returns>List of instances of <see cref="NavigationItem{TNavigationItem}"/>.</returns>
        IEnumerable<TNavigationItem> GetOrAdd<TNavigationItem>(Func<IEnumerable<TNavigationItem>> factory)
             where TNavigationItem : NavigationItem<TNavigationItem>, new();
    }
}
