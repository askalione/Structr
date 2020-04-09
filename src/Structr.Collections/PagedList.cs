using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Collections
{
    public class PagedList<T> : IPagedList<T>
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

        public PagedList(IEnumerable<T> collection, int totalItems, int pageNumber, int pageSize)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));
            if (totalItems < 0)
                throw new ArgumentOutOfRangeException(nameof(totalItems), totalItems, "Total numbers of elements in superset must be greater or equal 0");
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Page number must be greater or equal 1");
            if (pageSize < 0)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater or equal 0");            

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

    public static class PagedList
    {
        public static IPagedList<T> Empty<T>()
            => new PagedList<T>(Enumerable.Empty<T>(), 0, 1, 0);
    }
}
