using System;
using System.Collections.Generic;

namespace Structr.Collections
{
    /// <summary>
    /// Extension methods for <see cref="IPagedList"/>.
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// Creates new <see cref="PagedList{TDestination}"/> instance using elements from <paramref name="destination"/>
        /// and page parameters from <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TDestination">Destination items type.</typeparam>
        /// <param name="source">Base PagedList, whitch parameters should be used for new one.</param>
        /// <param name="destination">Array of items to build new PagedList with.</param>
        /// <returns>New instance of <see cref="PagedList{TDestination}"/> that contains items from <paramref name="destination"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
        public static PagedList<TDestination> ToPagedList<TDestination>(this IPagedList source, IEnumerable<TDestination> destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new PagedList<TDestination>(destination, source.TotalItems, source.PageNumber, source.PageSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializablePagedList{T}"/> class using elements
        /// and page parameters from <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="source">Source collection of elements.</param>
        /// <returns>Instance of the <see cref="SerializablePagedList{T}"/> class.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
        public static SerializablePagedList<T> ToSerializablePagedList<T>(this PagedList<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            SerializablePagedList<T> result = new SerializablePagedList<T>
            {
                Items = source,
                TotalItems = source.TotalItems,
                PageNumber = source.PageNumber,
                PageSize = source.PageSize
            };

            return result;
        }
    }
}
