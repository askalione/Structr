using Microsoft.EntityFrameworkCore;
using Structr.Abstractions;
using Structr.Collections;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Data.EFCore
{
    public static class QueryableExtensions
    {
        public static IPagedList<TSource> ToPagedList<TSource>(this IQueryable<TSource> source, int pageSize, int pageNumber)
        {
            Ensure.NotNull(source, nameof(source));

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater or equal 1");
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Page number must be greater or equal 1");

            var totalItems = source.Count();
            if (totalItems == 0)
                return PagedList.Empty<TSource>();

            var skip = (pageNumber - 1) * pageSize;
            return new PagedList<TSource>(source.Skip(skip).Take(pageSize).ToList(), totalItems, pageSize, pageNumber);
        }

        public static async Task<IPagedList<TSource>> ToPagedListAsync<TSource>(this IQueryable<TSource> source,
            int pageSize, int pageNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            Ensure.NotNull(source, nameof(source));

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Page size must be greater or equal 1");
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "Page number must be greater or equal 1");

            var totalItems = await source.CountAsync(cancellationToken).ConfigureAwait(false);
            if (totalItems == 0)
                return PagedList.Empty<TSource>();

            var skip = (pageNumber - 1) * pageSize;
            return new PagedList<TSource>(await source.Skip(skip).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false), totalItems, pageSize, pageNumber);
        }
    }
}
