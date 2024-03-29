using Structr.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Structr.EntityFramework
{
    /// <summary>
    /// Extensions methods for the <see cref="DbContext"/>.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Controls operations on entities. Records who and when created, modified, or deleted an entity.
        /// <br/>Records:
        /// <br/>DateCreated for <see cref="ICreatable"/>,
        /// <br/>CreatedBy for <see cref="ISignedCreatable"/>,
        /// <br/>DateModified for <see cref="IModifiable"/>,
        /// <br/>ModifiedBy for <see cref="ISignedModifiable"/>,
        /// <br/>DateDeleted for <see cref="ISoftDeletable"/>,
        /// <br/>DeletedBy for <see cref="ISignedSoftDeletable"/> entity.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="timestampProvider">The <see cref="AuditTimestampProvider"/>.</param>
        /// <param name="signProvider">The <see cref="AuditSignProvider"/>.</param>
        /// <returns>The <see cref="DbContext"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="context"/> is <see langword="null"/>.</exception>
        public static DbContext Audit(this DbContext context,
            AuditTimestampProvider timestampProvider = null,
            AuditSignProvider signProvider = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.ChangeTracker.DetectChanges();

            var entries = context.ChangeTracker.Entries<IAuditable>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            var timestamp = timestampProvider?.Invoke() ?? DateTime.Now;
            var sign = signProvider?.Invoke();

            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            OnAdded(entry, timestamp, sign);
                            break;
                        case EntityState.Modified:
                            OnModified(entry, timestamp, sign);
                            break;
                        case EntityState.Deleted:
                            OnDeleted(context, entry, timestamp, sign);
                            break;
                    }
                }
            }

            return context;
        }

        private static void OnAdded(DbEntityEntry entry, DateTime timestamp, string sign)
        {
            if (entry.Entity is ICreatable)
            {
                entry.Property(AuditableProperties.DateCreated).CurrentValue = timestamp;
                if (entry.Entity is ISignedCreatable)
                {
                    entry.Property(AuditableProperties.CreatedBy).CurrentValue = sign;
                }
            }

            if (entry.Entity is IModifiable)
            {
                entry.Property(AuditableProperties.DateModified).CurrentValue = timestamp;
                if (entry.Entity is ISignedModifiable)
                {
                    entry.Property(AuditableProperties.ModifiedBy).CurrentValue = sign;
                }
            }

            if (entry.Entity is ISoftDeletable)
            {
                entry.Property(AuditableProperties.DateDeleted).CurrentValue = null;
                if (entry.Entity is ISignedSoftDeletable)
                {
                    entry.Property(AuditableProperties.DeletedBy).CurrentValue = null;
                }
            }
        }

        private static void OnModified(DbEntityEntry entry, DateTime timestamp, string sign)
        {
            if (entry.Entity is ICreatable)
            {
                entry.Property(AuditableProperties.DateCreated).IsModified = false;
                if (entry.Entity is ISignedCreatable)
                    entry.Property(AuditableProperties.CreatedBy).IsModified = false;
            }

            if (entry.Entity is IModifiable)
            {
                entry.Property(AuditableProperties.DateModified).CurrentValue = timestamp;
                if (entry.Entity is ISignedModifiable)
                    entry.Property(AuditableProperties.ModifiedBy).CurrentValue = sign;
            }
        }

        private static void OnDeleted(DbContext context,
            DbEntityEntry entry,
            DateTime timestamp,
            string sign)
        {
            if (entry.Entity is ICreatable)
            {
                entry.Property(AuditableProperties.DateCreated).IsModified = false;
                if (entry.Entity is ISignedCreatable)
                {
                    entry.Property(AuditableProperties.CreatedBy).IsModified = false;
                }
            }

            if (entry.Entity is IModifiable)
            {
                entry.Property(AuditableProperties.DateModified).IsModified = false;
                if (entry.Entity is ISignedModifiable)
                {
                    entry.Property(AuditableProperties.ModifiedBy).IsModified = false;
                }
            }

            if (entry.Entity is IUndeletable)
            {
                if (entry.Entity is ISoftDeletable)
                {
                    entry.State = EntityState.Modified;

                    var entitySet = ((IObjectContextAdapter)context).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity).EntitySet;
                    IEnumerable<string> keyNames = entitySet.ElementType.KeyMembers.Select(x => x.Name).ToList();
                    foreach (string propName in entry.OriginalValues.PropertyNames)
                    {
                        if (keyNames.Contains(propName) == false)
                        {
                            entry.Property(propName).IsModified = false;
                        }
                    }

                    entry.Property(AuditableProperties.DateDeleted).IsModified = true;
                    entry.Property(AuditableProperties.DateDeleted).CurrentValue = timestamp;
                    if (entry.Entity is ISignedSoftDeletable)
                    {
                        entry.Property(AuditableProperties.DeletedBy).IsModified = true;
                        entry.Property(AuditableProperties.DeletedBy).CurrentValue = sign;
                    }
                }
                else
                {
                    entry.State = EntityState.Unchanged;
                }
            }
        }
    }
}
