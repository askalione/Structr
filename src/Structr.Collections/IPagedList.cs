using System.Collections;
using System.Collections.Generic;

namespace Structr.Collections
{
    /// <summary>
    /// Collection based type, suitable for pagination tasks. It supplies all
    /// needed properties such as page size, number, first and last page attribute, etc.
    /// </summary>
    /// <typeparam name="T">Type of items in list.</typeparam>
    public interface IPagedList : IEnumerable
    {
        /// <summary>
        /// Gets declared total count of items in superset collection.
        /// </summary>
        int TotalItems { get; }

        /// <summary>
        /// Gets current page number.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Gets page size.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Gets total count of pages.
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// Determines if there is a page before current.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Determines if there is a page after current.
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Determines whether current page is the first one.
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// Determines whether current page is the last one.
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// Gets number of first item on page.
        /// </summary>
        int FirstItemOnPage { get; }

        /// <summary>
        /// Gets number of last item on page.
        /// </summary>
        int LastItemOnPage { get; }
    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        T this[int index] { get; }

        /// <summary>
        /// Gets count of items on page.
        /// </summary>
        /// <remarks>Could be less then <see cref="PageSize"/> when <see cref="TotalItems"/>
        /// couldn't be divided without a remainder on <see cref="PageSize"/>.</remarks>
        int Count { get; }
    }
}
