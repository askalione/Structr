using System;
using System.Collections.Generic;

namespace Structr.Collections
{
    /// <summary>
    /// Extension methods for <see cref="IPagedEnumerable"/>.
    /// </summary>
    public static class PagedEnumerableExtensions
    {
        /// <summary>
        /// Creates new <see cref="PagedList{TDestination}"/> instance using elements from <paramref name="destination"/>
        /// and page parameters from <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TDestination">Destination items type.</typeparam>
        /// <param name="source">Base PagedList, whitch parameters should be used for new one.</param>
        /// <param name="destination">Collection of items to build new <see cref="PagedList{TDestination}"/> with.</param>
        /// <returns>New instance of <see cref="PagedList{TDestination}"/> that contains items from <paramref name="destination"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
        public static PagedList<TDestination> ToPagedList<TDestination>(this IPagedEnumerable source, IEnumerable<TDestination> destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new PagedList<TDestination>(destination, source.TotalItems, source.PageNumber, source.PageSize);
        }
    }
}
