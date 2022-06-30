using EntityFramework.DynamicFilters;
using Structr.Domain;
using Structr.EntityFramework.Internal;
using Structr.EntityFramework.Options;
using System;
using System.Data.Entity;

namespace Structr.EntityFramework
{
    /// <summary>
    /// Extensions for the <see cref="DbModelBuilder"/>.
    /// </summary>
    public static class DbModelBuilderExtensions
    {
        /// <inheritdoc cref="ApplyEntityConfiguration(DbModelBuilder, Action{EntityConfigurationOptions})"/>
        public static DbModelBuilder ApplyEntityConfiguration(this DbModelBuilder builder)
            => ApplyEntityConfiguration(builder, null);

        /// <summary>
        /// Applies the default configuration for all classes inherited from the <see cref="Entity{TEntity, TKey}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="DbModelBuilder"/>.</param>
        /// <param name="configureOptions">Delegate for additional configure options.</param>
        /// <returns>The <see cref="DbModelBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        public static DbModelBuilder ApplyEntityConfiguration(this DbModelBuilder builder, Action<EntityConfigurationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new EntityConfigurationOptions();

            configureOptions?.Invoke(options);

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

        /// <inheritdoc cref="ApplyValueObjectConfiguration(DbModelBuilder, Action{ValueObjectConfigurationOptions})"/>
        public static DbModelBuilder ApplyValueObjectConfiguration(this DbModelBuilder builder)
            => ApplyValueObjectConfiguration(builder, null);

        /// <summary>
        /// Applies the default configuration for all classes inherited from the <see cref="ValueObject{TValueObject}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="DbModelBuilder"/>.</param>
        /// <param name="configureOptions">Delegate for additional configure options.</param>
        /// <returns>The <see cref="DbModelBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        public static DbModelBuilder ApplyValueObjectConfiguration(this DbModelBuilder builder, Action<ValueObjectConfigurationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new ValueObjectConfigurationOptions();

            configureOptions?.Invoke(options);

            builder.Types().Where(x => typeof(ValueObject<>).IsAssignableFromGenericType(x)).Configure(x =>
            {
                if (options.Configure != null)
                {
                    options.Configure.Invoke(x);
                }
            });

            return builder;
        }

        /// <inheritdoc cref="ApplyAuditableConfiguration(DbModelBuilder, Action{AuditableConfigurationOptions})"/>
        public static DbModelBuilder ApplyAuditableConfiguration(this DbModelBuilder builder)
            => ApplyAuditableConfiguration(builder, null);

        /// <summary>
        /// Applies the default configuration for all classes that implement the <see cref="IAuditable"/>
        /// except inherited from the <see cref="ValueObject{TValueObject}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="DbModelBuilder"/>.</param>
        /// <param name="configureOptions">Delegate for additional configure options.</param>
        /// <returns>The <see cref="DbModelBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="builder"/> is <see langword="null"/>.</exception>
        public static DbModelBuilder ApplyAuditableConfiguration(this DbModelBuilder builder, Action<AuditableConfigurationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new AuditableConfigurationOptions();

            configureOptions?.Invoke(options);

            builder.Types().Where(x => typeof(IAuditable).IsAssignableFrom(x)).Configure(x =>
            {
                Type entityClrType = x.ClrType;

                if (typeof(ICreatable).IsAssignableFrom(entityClrType))
                {
                    x.Property(AuditableProperties.DateCreated)
                        .IsRequired();
                    if (typeof(ISignedCreatable).IsAssignableFrom(entityClrType))
                    {
                        x.Property(AuditableProperties.CreatedBy)
                            .IsRequired(options.SignedColumnIsRequired)
                            .HasMaxLength(options.SignedColumnMaxLength);
                    }
                }

                if (typeof(IModifiable).IsAssignableFrom(entityClrType))
                {
                    x.Property(AuditableProperties.DateModified)
                        .IsRequired(true);
                    if (typeof(ISignedModifiable).IsAssignableFrom(entityClrType))
                    {
                        x.Property(AuditableProperties.ModifiedBy)
                            .IsRequired(options.SignedColumnIsRequired)
                            .HasMaxLength(options.SignedColumnMaxLength);
                    }
                }

                if (typeof(ISoftDeletable).IsAssignableFrom(entityClrType))
                {
                    x.Property(AuditableProperties.DateDeleted)
                        .IsRequired(false);
                    if (typeof(ISignedSoftDeletable).IsAssignableFrom(entityClrType))
                    {
                        x.Property(AuditableProperties.DeletedBy)
                            .IsRequired(false)
                            .HasMaxLength(options.SignedColumnMaxLength);
                    }
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
