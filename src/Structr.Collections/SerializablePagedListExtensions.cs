using System;

namespace Structr.Collections
{
    /// <summary>
    /// Extension methods for <see cref="SerializablePagedList{T}"/>.
    /// </summary>
    public static class SerializablePagedListExtensions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class based on serializable source collection.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <param name="source">The <see cref="SerializablePagedList{T}"/>.</param>
        /// <returns>Instance of the <see cref="PagedList{T}"/> class.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
        public static PagedList<T> ToPagedList<T>(this SerializablePagedList<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            PagedList<T> result = new PagedList<T>(source.Items,
                source.TotalItems,
                source.PageNumber,
                source.PageSize);

            return result;
        }
    }
}
