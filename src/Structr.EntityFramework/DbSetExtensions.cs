using System;
using System.Data.Entity;
using System.Reflection;

namespace Structr.EntityFramework
{
    public static class DbSetExtensions
    {
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
