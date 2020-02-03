using EntityFramework.DynamicFilters;
using Structr.Abstractions;
using Structr.Abstractions.Extensions;
using Structr.Domain;
using Structr.EntityFramework.Options;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Structr.EntityFramework
{
    public static class DbModelBuilderExtensions
    {
        public static DbModelBuilder ApplyEntityConfiguration(this DbModelBuilder builder)
            => ApplyEntityConfiguration(builder, null);

        public static DbModelBuilder ApplyEntityConfiguration(this DbModelBuilder builder, Action<EntityConfigurationOptions> configureOptions)
        {
            Ensure.NotNull(builder, nameof(builder));

            var options = new EntityConfigurationOptions();

            configureOptions?.Invoke(options);

            var test = builder.Types();

            builder.Types().Where(x => typeof(Entity<,>).IsAssignableFromGenericType(x)).Configure(x =>
            {
                x.HasKey(nameof(Entity.Id));
            });

            if (options.Configure != null)
            {
                builder.Types().Where(x => typeof(Entity<>).IsAssignableFromGenericType(x)).Configure(x =>
                {
                    options.Configure.Invoke(x);
                });
            }

            return builder;
        }

        public static DbModelBuilder ApplyValueObjectConfiguration(this DbModelBuilder builder)
            => ApplyValueObjectConfiguration(builder, null);

        public static DbModelBuilder ApplyValueObjectConfiguration(this DbModelBuilder builder, Action<ValueObjectConfigurationOptions> configureOptions)
        {
            Ensure.NotNull(builder, nameof(builder));

            var options = new ValueObjectConfigurationOptions();

            configureOptions?.Invoke(options);

            builder.Types().Where(x => typeof(ValueObject<>).IsAssignableFromGenericType(x)).Configure(x =>
            {
                if (options.Configure != null)
                {
                    options.Configure.Invoke(x);
                }

                if (options.ScalarPropertyNameFactory != null)
                {
                    var props = x.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => (!p.PropertyType.IsClass || p.PropertyType == typeof(String)) && !p.PropertyType.IsInterface);
                    foreach (var prop in x.ClrType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        x.Property(prop).HasColumnName(options.ScalarPropertyNameFactory(x, prop));
                    }
                }
            });

            return builder;
        }

        public static DbModelBuilder ApplyAuditableConfiguration(this DbModelBuilder builder)
            => ApplyAuditableConfiguration(builder, null);

        public static DbModelBuilder ApplyAuditableConfiguration(this DbModelBuilder builder, Action<AuditableConfigurationOptions> configureOptions)
        {
            Ensure.NotNull(builder, nameof(builder));

            var options = new AuditableConfigurationOptions();

            configureOptions?.Invoke(options);

            builder.Types().Where(x => typeof(IAuditable).IsAssignableFrom(x)).Configure(x =>
            {
                var entityClrType = x.ClrType;

                if (typeof(ICreatable).IsAssignableFrom(entityClrType))
                {
                    x.Property(AuditableProperties.DateCreated)
                        .IsRequired();
                    if (typeof(ISignedCreatable).IsAssignableFrom(entityClrType))
                        x.Property(AuditableProperties.CreatedBy)
                            .IsRequired(options.SignedColumnIsRequired)
                            .HasMaxLength(options.SignedColumnMaxLength);
                }

                if (typeof(IModifiable).IsAssignableFrom(entityClrType))
                {
                    x.Property(AuditableProperties.DateModified)
                        .IsRequired(true);
                    if (typeof(ISignedModifiable).IsAssignableFrom(entityClrType))
                        x.Property(AuditableProperties.ModifiedBy)
                            .IsRequired(options.SignedColumnIsRequired)
                            .HasMaxLength(options.SignedColumnMaxLength);
                }

                if (typeof(ISoftDeletable).IsAssignableFrom(entityClrType))
                {
                    x.Property(AuditableProperties.DateDeleted)
                        .IsRequired(false);
                    if (typeof(ISignedSoftDeletable).IsAssignableFrom(entityClrType))
                        x.Property(AuditableProperties.DeletedBy)
                            .IsRequired(options.SignedColumnIsRequired)
                            .HasMaxLength(options.SignedColumnMaxLength);
                }
            });

            builder.Filter(options.SoftDeletableFilterName, (ISoftDeletable entity) => entity.DateDeleted, null);

            return builder;
        }

        private abstract class Entity : Entity<Entity, Guid>
        {
        }
    }
}
