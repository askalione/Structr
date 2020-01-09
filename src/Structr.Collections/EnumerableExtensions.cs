using System.Collections.Generic;
using System.Linq;

namespace Structr.Collections
{
    public static class EnumerableExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int pageSize, int pageNumber)
        {
            return ToPagedList(collection, collection.Count(), pageSize, pageNumber);
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> collection, int totalItems, int pageSize, int pageNumber)
        {
            return new PagedList<T>(collection, totalItems, pageSize, pageNumber);
        }
    }
}
