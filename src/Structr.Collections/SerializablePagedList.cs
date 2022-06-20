using System.Collections.Generic;

namespace Structr.Collections
{
    /// <summary>
    /// Class for serialize and deserialize a <see cref="PagedList{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of items in list.</typeparam>
    public class SerializablePagedList<T>
    {
        /// <summary>
        /// Collection of elements.
        /// </summary>
        public IEnumerable<T> Items { get; set; }

        /// <summary>
        /// Gets declared total count of items in superset collection.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets current page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets page size.
        /// </summary>
        public int PageSize { get; set; }
    }
}
