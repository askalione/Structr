using System.Collections.Generic;
using System.Linq;

namespace Structr.Collections
{
    public static class EnumerableExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            return ToPagedList(collection, collection.Count(), pageNumber, pageSize);
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int totalItems, int pageNumber, int pageSize)
        {
            return new PagedList<T>(collection, totalItems, pageNumber, pageSize);
        }
    }
}
