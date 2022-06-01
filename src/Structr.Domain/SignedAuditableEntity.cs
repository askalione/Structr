namespace Structr.Domain
{
    /// <summary>
    /// General class for a signed auditable entity <see cref="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class SignedAuditableEntity<TEntity> : AuditableEntity<TEntity>, ISignedCreatable, ISignedModifiable
        where TEntity : SignedAuditableEntity<TEntity>
    {
        public string CreatedBy { get; protected set; }
        public string ModifiedBy { get; protected set; }

        protected SignedAuditableEntity() : base() { }
    }

    /// <summary>
    /// General class for a signed auditable entity <see cref="TEntity"/> with identifier <see cref="TKey"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <typeparam name="TKey">Type of entity identifier.</typeparam>
    public abstract class SignedAuditableEntity<TEntity, TKey> : AuditableEntity<TEntity, TKey>, ISignedCreatable, ISignedModifiable
        where TEntity : SignedAuditableEntity<TEntity, TKey>
    {
        public string CreatedBy { get; protected set; }
        public string ModifiedBy { get; protected set; }

        protected SignedAuditableEntity() : base() { }
    }
}
