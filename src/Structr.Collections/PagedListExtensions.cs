using System;
using System.Collections.Generic;

namespace Structr.Collections
{
    public static class PagedListExtensions
    {
        public static IPagedList<TDestination> ToPagedList<TDestination>(this IPagedList source, IEnumerable<TDestination> destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new PagedList<TDestination>(destination, source.TotalItems, source.PageSize, source.PageNumber);
        }

        // TODO: Move to EFCore project
        //public static IPagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageSize, int pageNumber)
        //{
        //    if (query == null)
        //        throw new ArgumentNullException(nameof(query));

        //    int take = pageSize;
        //    int skip = pageNumber > 0 ? (pageNumber - 1) * pageSize : 0;

        //    var totalItems = query.Count();
        //    return totalItems > 0
        //        ? new PagedList<T>(query.Skip(() => skip).Take(take).ToList(), totalItems, pageSize, pageNumber)
        //        : PagedList.Empty<T>();
        //}
    }
}
