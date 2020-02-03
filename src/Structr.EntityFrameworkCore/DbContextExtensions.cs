using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Structr.Abstractions;
using Structr.Abstractions.Providers;
using Structr.Domain;
using System;
using System.Linq;
using System.Security.Principal;

namespace Structr.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static DbContext Audit(this DbContext context, ITimestampProvider timestampProvider = null, IPrincipal principal = null)
        {
            Ensure.NotNull(context, nameof(context));

            context.ChangeTracker.DetectChanges();

            var entries = context.ChangeTracker.Entries<IAuditable>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            var timestamp = timestampProvider?.GetTimestamp() ?? DateTime.Now;
            var sign = principal?.Identity?.Name;

            if (entries != null)
            {
                foreach (var entry in entries)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            OnAdded(context, entry, timestamp, sign);
                            break;
                        case EntityState.Modified:
                            OnModified(context, entry, timestamp, sign);
                            break;
                        case EntityState.Deleted:
                            OnDeleted(context, entry, timestamp, sign);
                            break;
                    }
                }
            }

            return context;
        }

        private static void OnAdded(DbContext context, EntityEntry entry, DateTime timestamp, string sign)
        {
            if (entry.Entity is ICreatable)
            {
                entry.Property(AuditableProperties.DateCreated).CurrentValue = timestamp;
                if (entry.Entity is ISignedCreatable)
                    entry.Property(AuditableProperties.CreatedBy).CurrentValue = sign;
            }

            if (entry.Entity is IModifiable)
            {
                entry.Property(AuditableProperties.DateModified).CurrentValue = timestamp;
                if (entry.Entity is ISignedModifiable)
                    entry.Property(AuditableProperties.ModifiedBy).CurrentValue = sign;
            }

            if (entry.Entity is ISoftDeletable)
            {
                entry.Property(AuditableProperties.DateDeleted).CurrentValue = null;
                if (entry.Entity is ISignedSoftDeletable)
                    entry.Property(AuditableProperties.DeletedBy).CurrentValue = null;
            }
        }

        private static void OnModified(DbContext context, EntityEntry entry, DateTime timestamp, string sign)
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

        private static void OnDeleted(DbContext context, EntityEntry entry, DateTime timestamp, string sign)
        {
            if (entry.Entity is ICreatable)
            {
                entry.Property(AuditableProperties.DateCreated).IsModified = false;
                if (entry.Entity is ISignedCreatable)
                    entry.Property(AuditableProperties.CreatedBy).IsModified = false;
            }

            if (entry.Entity is IModifiable)
            {
                entry.Property(AuditableProperties.DateModified).IsModified = false;
                if (entry.Entity is ISignedModifiable)
                    entry.Property(AuditableProperties.ModifiedBy).IsModified = false;
            }

            if (entry.Entity is IUndeletable)
            {
                entry.State = EntityState.Unchanged;

                if (entry.Entity is ISoftDeletable)
                {
                    var entityPropName = AuditableProperties.DateDeleted;
                    entry.Property(entityPropName).IsModified = true;
                    entry.Property(entityPropName).CurrentValue = timestamp;
                    if (entry.Entity is ISignedSoftDeletable)
                    {
                        entityPropName = AuditableProperties.DeletedBy;
                        entry.Property(entityPropName).IsModified = true;
                        entry.Property(entityPropName).CurrentValue = sign;
                    }
                }
            }
        }
    }
}
