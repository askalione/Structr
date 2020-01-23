using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Structr.Abstractions;
using Structr.Abstractions.Providers;
using Structr.Domain;
using System;
using System.Linq;
using System.Security.Principal;

namespace Structr.Data.EFCore
{
    public static class DbContextExtensions
    {
        public static DbContext UseAudit(this DbContext context, bool audit = true)
        {
            Ensure.NotNull(context, nameof(context));

            var changeTracker = context.ChangeTracker;

            if (audit)
            {
                changeTracker.Tracked += OnEntityTracked;
                changeTracker.StateChanged += OnEntityStateChanged;
            }
            else
            {
                changeTracker.Tracked -= OnEntityTracked;
                changeTracker.StateChanged -= OnEntityStateChanged;
            }

            return context;
        }

        private static void OnEntityTracked(object sender, EntityTrackedEventArgs e)
        {
            var entry = e.Entry;

            if (!e.FromQuery && entry.State == EntityState.Added)
            {
                var timeStamp = entry.GetTimestamp();
                var sign = entry.GetSign();

                if (entry.Entity is ICreatable)
                {
                    entry.Property(AuditableProperties.DateCreated).CurrentValue = timeStamp;
                    if (entry.Entity is ISignedCreatable)
                        entry.Property(AuditableProperties.CreatedBy).CurrentValue = sign;
                }

                if (entry.Entity is IModifiable)
                {
                    entry.Property(AuditableProperties.DateModified).CurrentValue = timeStamp;
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
        }

        private static void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e)
        {
            var entry = e.Entry;

            if (e.NewState == EntityState.Modified)
            {
                if (entry.Entity is ICreatable)
                {
                    entry.Property(AuditableProperties.DateCreated).IsModified = false;
                    if (entry.Entity is ISignedCreatable)
                        entry.Property(AuditableProperties.CreatedBy).IsModified = false;
                }

                if (entry.Entity is IModifiable)
                {
                    var timeStamp = entry.GetTimestamp();
                    entry.Property(AuditableProperties.DateModified).CurrentValue = timeStamp;
                    if (entry.Entity is ISignedModifiable)
                    {
                        var sign = entry.GetSign();
                        entry.Property(AuditableProperties.ModifiedBy).CurrentValue = sign;
                    }
                }

                if (entry.Entity is ISoftDeletable)
                {
                    entry.Property(AuditableProperties.DateDeleted).IsModified = false;
                    if (entry.Entity is ISignedSoftDeletable)
                        entry.Property(AuditableProperties.DeletedBy).IsModified = false;
                }
            }

            if (e.NewState == EntityState.Deleted)
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

                    var timeStamp = entry.GetTimestamp();
                    var propName = AuditableProperties.DateDeleted;
                    entry.Property(propName).IsModified = true;
                    entry.Property(propName).CurrentValue = timeStamp;
                    if (entry.Entity is ISignedSoftDeletable)
                    {
                        var sign = entry.GetSign();
                        propName = AuditableProperties.DeletedBy;
                        entry.Property(propName).IsModified = true;
                        entry.Property(propName).CurrentValue = sign;
                    }
                }
            }
        }

        private static DateTime GetTimestamp(this EntityEntry entry)
        {
            var serviceProvider = entry.Context.GetInfrastructure();
            var timestampProvider = (ITimestampProvider)serviceProvider.GetService(typeof(ITimestampProvider));
            return timestampProvider?.GetTimestamp() ?? DateTime.Now;
        }

        private static string GetSign(this EntityEntry entry)
        {
            var serviceProvider = entry.Context.GetInfrastructure();
            var principal = (IPrincipal)serviceProvider.GetService(typeof(IPrincipal));
            return principal?.Identity?.Name;
        }
    }
}
