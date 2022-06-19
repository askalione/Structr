using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Collections
{
    /// <summary>
    /// Collection based type, suitable for pagination tasks. It supplies all
    /// needed properties such as page size, number, first and last page attribute, etc.
    /// </summary>
    /// <typeparam name="T">Type of items in list.</typeparam>
    public class PagedList<T> : IPagedEnumerable, IEnumerable<T>
    {
        public int TotalItems { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }
        public bool IsFirstPage { get; }
        public bool IsLastPage { get; }
        public int FirstItemOnPage { get; }
        public int LastItemOnPage { get; }

        public T this[int index] => _collection[index];

        public int Count => _collection.Count;

        private readonly List<T> _collection;

        /// <summary>
        /// Initializes a new instance of the Structr.Collections.PagedList`1 class that
        /// contains no elements.
        /// </summary>
        public PagedList() : this(Enumerable.Empty<T>(), 0, 1, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedList{T}"/> class that
        /// contains elements copied from the specified collection. It's intended to contain
        /// currently displaying elements from corresponding superset.
        /// </summary>
        /// <param name="collection">Source colleciton of elements. For example: part of search results to be displayed.</param>
        /// <param name="totalItems">Total count of items in superset. For example: total count of search results.</param>
        /// <param name="pageNumber">Number of current page.</param>
        /// <param name="pageSize">Count of items to be dislpayed on page.</param>
        /// <exception cref="ArgumentNullException">Null source collection was provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">There are some inconsistence in provided page prameters.</exception>
        public PagedList(IEnumerable<T> collection, int totalItems, int pageNumber, int pageSize)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (totalItems < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(totalItems), totalItems,
                    "Total number of elements in superset must be greater or equal 0");
            }
            if (totalItems < collection.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(totalItems), totalItems,
                    "Total number of elements in superset must be greater or equal collection items count");
            }
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber,
                    "Page number must be greater or equal 1");
            }
            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize,
                    "Page size must be greater or equal 0");
            }
            if (pageSize < collection.Count())
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize,
                    "Page size must be greater or equal collection items count");
            }

            _collection = collection.ToList();

            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;

            TotalPages = TotalItems > 0 ? (int)(Math.Ceiling(TotalItems / (double)PageSize)) : 0;
            HasPreviousPage = PageNumber > 1 && PageNumber <= TotalPages;
            HasNextPage = PageNumber < TotalPages;
            IsFirstPage = TotalPages > 0 && PageNumber == 1;
            IsLastPage = TotalPages > 0 && PageNumber == TotalPages;
            FirstItemOnPage = TotalPages > 0 ? (PageNumber - 1) * PageSize + 1 : 0;

            int lastItemOnPage = FirstItemOnPage + PageSize - 1;
            LastItemOnPage = TotalPages > 0
                ? (lastItemOnPage > TotalItems ? TotalItems : lastItemOnPage)
                : 0;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    /// <summary>
    /// Static class providing basic fuctionality for typed PageLists creation.
    /// </summary>
    public static class PagedList
    {
        /// <summary>
        /// Creates an empty paged list.
        /// </summary>
        /// <typeparam name="T">Type of items in list.</typeparam>
        /// <returns></returns>
        public static PagedList<T> Empty<T>()
            => new PagedList<T>();
    }
}
