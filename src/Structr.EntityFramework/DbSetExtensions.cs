using System;
using System.Data.Entity;
using System.Reflection;

namespace Structr.EntityFramework
{
    /// <summary>
    /// Extensions methods for the <see cref="DbSet{TEntity}"/>.
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// Returns <see cref="DbContext"/> for the <see cref="DbSet{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">Some entity type.</typeparam>
        /// <param name="entry">The <see cref="DbSet{TEntity}"/>.</param>
        /// <returns>The <see cref="DbContext"/>.</returns>
        public static DbContext GetContext<TEntity>(this DbSet<TEntity> entry) where TEntity : class
        {
            object internalSet = entry
                .GetType()
                .GetField("_internalSet", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(entry);
            object internalContext = internalSet
                .GetType()
                .BaseType
                .GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(internalSet);
            return (DbContext)internalContext
                .GetType()
                .GetProperty("Owner", BindingFlags.Instance | BindingFlags.Public)
                .GetValue(internalContext, null);
        }

        /// <summary>
        /// Sets the <see cref="EntityState.Modified"/> state to an <paramref name="entity"/>.
        /// </summary>
        /// <typeparam name="TEntity">Some entity type.</typeparam>
        /// <param name="entry">The <see cref="DbSet{TEntity}"/>.</param>
        /// <param name="entity">The entity.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="entry"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="entity"/> is <see langword="null"/>.</exception>
        public static void Update<TEntity>(this DbSet<TEntity> entry, TEntity entity) where TEntity : class
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            entry.GetContext().Entry(entity).State = EntityState.Modified;
        }
    }
}
