using System;

namespace Structr.Domain
{
    /// <summary>
    /// General class for an auditable entity <see cref="TEntity"/>.
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
    /// General class for an auditable entity <see cref="TEntity"/> with identifier <see cref="TKey"/>.
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
