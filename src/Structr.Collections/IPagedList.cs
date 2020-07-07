using System.Collections;
using System.Collections.Generic;

namespace Structr.Collections
{
    public interface IPagedList : IEnumerable
    {
        int TotalItems { get; }
        int PageNumber { get; }
        int PageSize { get; }                     
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        bool IsFirstPage { get; }
        bool IsLastPage { get; }
        int FirstItemOnPage { get; }
        int LastItemOnPage { get; }
    }

    public interface IPagedList<out T> : IPagedList, IEnumerable<T>
    {
        T this[int index] { get; }
        int Count { get; }
    }
}
