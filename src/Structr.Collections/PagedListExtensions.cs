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

            return new PagedList<TDestination>(destination, source.TotalItems, source.PageNumber, source.PageSize);
        }
    }
}
