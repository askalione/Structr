using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Structr.Domain;
using Structr.EntityFrameworkCore.Internal;
using Structr.EntityFrameworkCore.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Structr.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyEntityConfiguration(this ModelBuilder builder)
            => ApplyEntityConfiguration(builder, null);

        public static ModelBuilder ApplyEntityConfiguration(this ModelBuilder builder, Action<EntityConfigurationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new EntityConfigurationOptions();

            configureOptions?.Invoke(options);

            foreach (var entityType in builder.GetEntityTypes(typeof(Entity<,>)))
            {
                builder.Entity(entityType.ClrType, entityTypeBuilder =>
                {
                    entityTypeBuilder.HasKey(nameof(Entity.Id));
                });
            }

            if (options.Configure != null)
            {
                foreach (var entityType in builder.GetEntityTypes(typeof(Entity<>)))
                {
                    builder.Entity(entityType.ClrType, entityTypeBuilder =>
                    {
                        options.Configure.Invoke(entityType, entityTypeBuilder);
                    });
                }
            }

            return builder;
        }

        public static ModelBuilder ApplyValueObjectConfiguration(this ModelBuilder builder)
            => ApplyValueObjectConfiguration(builder, null);

        public static ModelBuilder ApplyValueObjectConfiguration(this ModelBuilder builder, Action<ValueObjectConfigurationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new ValueObjectConfigurationOptions();

            configureOptions?.Invoke(options);

            foreach (var entityType in builder.GetEntityTypes(typeof(ValueObject<>)))
            {
                if (options.Configure != null)
                {
                    builder.Entity(entityType.ClrType, entityTypeBuilder =>
                    {
                        options.Configure.Invoke(entityType, entityTypeBuilder);
                    });
                }
            }

            return builder;
        }

        public static ModelBuilder ApplyAuditableConfiguration(this ModelBuilder builder)
            => ApplyAuditableConfiguration(builder, null);

        public static ModelBuilder ApplyAuditableConfiguration(this ModelBuilder builder, Action<AuditableConfigurationOptions> configureOptions)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = new AuditableConfigurationOptions();

            configureOptions?.Invoke(options);

            foreach (var entityType in builder.GetEntityTypes(typeof(IAuditable), x => !typeof(ValueObject<>).IsAssignableFromGenericType(x.ClrType)))
            {
                var entityClrType = entityType.ClrType;

                builder.Entity(entityClrType, entityTypeBuilder =>
                {
                    if (typeof(ICreatable).IsAssignableFrom(entityClrType))
                    {
                        entityTypeBuilder.Property(AuditableProperties.DateCreated)
                            .IsRequired(true);
                        if (typeof(ISignedCreatable).IsAssignableFrom(entityClrType))
                        {
                            entityTypeBuilder.Property(AuditableProperties.CreatedBy)
                                .IsRequired(options.SignedColumnIsRequired)
                                .HasMaxLength(options.SignedColumnMaxLength);
                        }
                    }

                    if (typeof(IModifiable).IsAssignableFrom(entityClrType))
                    {
                        entityTypeBuilder.Property(AuditableProperties.DateModified)
                            .IsRequired(true);
                        if (typeof(ISignedModifiable).IsAssignableFrom(entityClrType))
                        {
                            entityTypeBuilder.Property(AuditableProperties.ModifiedBy)
                                .IsRequired(options.SignedColumnIsRequired)
                                .HasMaxLength(options.SignedColumnMaxLength);
                        }
                    }

                    if (typeof(ISoftDeletable).IsAssignableFrom(entityClrType))
                    {
                        entityTypeBuilder.Property(AuditableProperties.DateDeleted)
                            .IsRequired(false);
                        entityTypeBuilder.HasSoftDeletableQueryFilter(entityClrType);

                        if (typeof(ISignedSoftDeletable).IsAssignableFrom(entityClrType))
                        {
                            entityTypeBuilder.Property(AuditableProperties.DeletedBy)
                                .IsRequired(options.SignedColumnIsRequired)
                                .HasMaxLength(options.SignedColumnMaxLength);
                        }
                    }
                });
            }

            return builder;
        }

        private static void HasSoftDeletableQueryFilter(this EntityTypeBuilder builder, Type entityClrType)
        {
            var param = Expression.Parameter(entityClrType);
            var propMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(DateTime?));
            var propMethodCall = Expression.Call(propMethodInfo, param, Expression.Constant(AuditableProperties.DateDeleted));
            BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, propMethodCall, Expression.Constant(null));
            var filter = Expression.Lambda(compareExpression, param);
            builder.HasQueryFilter(filter);
        }

        public static List<IMutableEntityType> GetEntityTypes(this ModelBuilder builder, Type type)
            => GetEntityTypes(builder, type, null);

        public static List<IMutableEntityType> GetEntityTypes(this ModelBuilder builder, Type type, Func<IMutableEntityType, bool> filter)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var types = builder.Model.GetEntityTypes()
                .Where(x => type.IsGenericType ? type.IsAssignableFromGenericType(x.ClrType) : type.IsAssignableFrom(x.ClrType));

            if (filter != null)
            {
                types = types.Where(x => filter(x));
            }

            return types.ToList();
        }

        private abstract class Entity : Entity<Entity, Guid>
        {
        }
    }
}
