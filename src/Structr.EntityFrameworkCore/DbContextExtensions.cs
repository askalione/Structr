using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        public static DbContext Audit(this DbContext context)
        {
            Ensure.NotNull(context, nameof(context));

            var entries = context.ChangeTracker.Entries<IAuditable>()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted);

            var timestamp = context.GetTimestamp();
            var sign = context.GetSign();

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
                            OnDeleted(entry, timestamp, sign);
                            break;
                    }
                }
            }

            return context;
        }

        private static void OnAdded(EntityEntry entry, DateTime timestamp, string sign)
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

        private static void OnModified(EntityEntry entry, DateTime timestamp, string sign)
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

            if (entry.Entity is ISoftDeletable)
            {
                entry.Property(AuditableProperties.DateDeleted).IsModified = false;
                if (entry.Entity is ISignedSoftDeletable)
                    entry.Property(AuditableProperties.DeletedBy).IsModified = false;
            }
        }

        private static void OnDeleted(EntityEntry entry, DateTime timestamp, string sign)
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

            if (entry.Entity is ISoftDeletable)
            {
                entry.State = EntityState.Modified;

                var props = entry.Properties.Where(x => !x.Metadata.IsPrimaryKey());
                foreach (var prop in props)
                    prop.IsModified = false;

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

        private static DateTime GetTimestamp(this DbContext context)
        {
            var serviceProvider = context.GetInfrastructure();
            var timestampProvider = (ITimestampProvider)serviceProvider.GetService(typeof(ITimestampProvider));
            return timestampProvider?.GetTimestamp() ?? DateTime.Now;
        }

        private static string GetSign(this DbContext context)
        {
            var serviceProvider = context.GetInfrastructure();
            var principal = (IPrincipal)serviceProvider.GetService(typeof(IPrincipal));
            return principal?.Identity?.Name;
        }
    }
}
