namespace Structr.Domain
{
    public abstract class SignedAuditableEntity<TEntity> : AuditableEntity<TEntity>, ISignedCreatable, ISignedModifiable
        where TEntity : SignedAuditableEntity<TEntity>
    {
        public string CreatedBy { get; protected set; }
        public string ModifiedBy { get; protected set; }

        protected SignedAuditableEntity() : base() { }
    }

    public abstract class SignedAuditableEntity<TEntity, TKey> : AuditableEntity<TEntity, TKey>, ISignedCreatable, ISignedModifiable
        where TEntity : SignedAuditableEntity<TEntity, TKey>
    {
        public string CreatedBy { get; protected set; }
        public string ModifiedBy { get; protected set; }

        protected SignedAuditableEntity() : base() { }
    }
}
