using System;

namespace Structr.Domain
{
    public abstract class AuditableEntity<TEntity> : Entity<TEntity>, ICreatable, IModifiable
        where TEntity : AuditableEntity<TEntity>
    {
        public DateTime DateCreated { get; protected set; }
        public DateTime DateModified { get; protected set; }

        protected AuditableEntity() : base() { }
    }

    public abstract class AuditableEntity<TEntity, TKey> : Entity<TEntity, TKey>, ICreatable, IModifiable
        where TEntity : AuditableEntity<TEntity, TKey>
    {
        public DateTime DateCreated { get; protected set; }
        public DateTime DateModified { get; protected set; }

        protected AuditableEntity() : base() { }
    }
}
