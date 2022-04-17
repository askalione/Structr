using Microsoft.EntityFrameworkCore;
using Structr.Collections;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.EntityFrameworkCore
{
    public static class QueryableExtensions
    {
        public static PagedList<TSource> ToPagedList<TSource>(this IQueryable<TSource> source, int pageNumber, int pageSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var totalItems = source.Count();
            if (totalItems == 0)
            {
                return new PagedList<TSource>();
            }

            var skip = pageNumber > 0 && pageSize > 0 ? (pageNumber - 1) * pageSize : 0;
            var take = pageSize > 0 ? pageSize : 0;

            if (skip > 0)
            {
                source = source.Skip(skip);
            }
            if (take > 0)
            {
                source = source.Take(take);
            }

            return new PagedList<TSource>(source.ToList(), totalItems, pageNumber, pageSize > 0 ? pageSize : totalItems);
        }

        public static async Task<PagedList<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> source,
            int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var totalItems = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            if (totalItems == 0)
            {
                return new PagedList<TSource>();
            }

            var skip = pageNumber > 0 && pageSize > 0 ? (pageNumber - 1) * pageSize : 0;
            var take = pageSize > 0 ? pageSize : 0;

            if (skip > 0)
            {
                source = source.Skip(skip);
            }
            if (take > 0)
            {
                source = source.Take(take);
            }

            return new PagedList<TSource>(await source.ToListAsync(cancellationToken).ConfigureAwait(false),
                totalItems, pageNumber, pageSize > 0 ? pageSize : totalItems);
        }
    }
}
