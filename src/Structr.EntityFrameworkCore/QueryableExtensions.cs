using Microsoft.EntityFrameworkCore;
using Structr.Collections;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.EntityFrameworkCore
{
    /// <summary>
    /// Extensions for the <see cref="IQueryable{TSource}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Synchronous conversion method from the <see cref="IQueryable{TSource}"/> to the <see cref="PagedList{TSource}"/>.
        /// </summary>
        /// <typeparam name="TSource">A collection item type.</typeparam>
        /// <param name="source">The <see cref="IQueryable{TSource}"/>.</param>
        /// <param name="pageNumber">The number of page.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The <see cref="PagedList{TSource}"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null"/>.</exception>
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

        /// <summary>
        /// Asynchronous conversion method from the <see cref="IQueryable{TSource}"/> to the <see cref="PagedList{TSource}"/>.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <inheritdoc cref="ToPagedList{TSource}(IQueryable{TSource}, int, int)"/>
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
