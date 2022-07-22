using System;

namespace Structr.Domain
{
    /// <summary>
    /// Base class for an auditable entity <typeparamref name="TEntity"/>. Provides DateCreated and DateModified auditable properties
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class AuditableEntity<TEntity> : Entity<TEntity>, ICreatable, IModifiable
        where TEntity : AuditableEntity<TEntity>
    {
        public DateTime DateCreated { get; protected set; }
        public DateTime DateModified { get; protected set; }

        protected AuditableEntity() : base() { }
    }

    /// <summary>
    /// Base class for an auditable entity <typeparamref name="TEntity"/> with identifier. Provides <see cref="DateCreated"/> and <see cref="DateModified"/> auditable properties
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <typeparam name="TKey">Type of entity identifier.</typeparam>
    public abstract class AuditableEntity<TEntity, TKey> : Entity<TEntity, TKey>, ICreatable, IModifiable
        where TEntity : AuditableEntity<TEntity, TKey>
    {
        public DateTime DateCreated { get; protected set; }
        public DateTime DateModified { get; protected set; }

        protected AuditableEntity() : base() { }
    }
}
