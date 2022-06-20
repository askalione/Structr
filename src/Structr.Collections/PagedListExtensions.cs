using System;
using System.Collections.Generic;

namespace Structr.Collections
{
    public static class PagedListExtensions
    {
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
