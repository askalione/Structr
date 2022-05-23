using System.Collections.Generic;
using System.Linq;

namespace Structr.Collections
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class based on source collection.
        /// TotalItems property will be automaticaly set to provided 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Source colleciton of elements. For example: part of search results to be displayed.</param>
        /// <param name="pageNumber">Number of current page.</param>
        /// <param name="pageSize">Count of items to be dislpayed on page.</param>
        /// <returns>Instance of the <see cref="PagedList{T}"/> class.</returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            return ToPagedList(collection, collection.Count(), pageNumber, pageSize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class based on source collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">Source colleciton of elements. For example: part of search results to be displayed.</param>
        /// <param name="totalItems">Total count of items in superset. For example: total count of search results.</param>
        /// <param name="pageNumber">Number of current page.</param>
        /// <param name="pageSize">Count of items to be dislpayed on page.</param>
        /// <returns>Instance of the <see cref="PagedList{T}"/> class.</returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int totalItems, int pageNumber, int pageSize)
        {
            return new PagedList<T>(collection, totalItems, pageNumber, pageSize);
        }
    }
}
