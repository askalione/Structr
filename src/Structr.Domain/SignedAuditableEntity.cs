namespace Structr.Domain
{
    /// <summary>
    /// Base class for a signed auditable entity <typeparamref name="TEntity"/>.
    /// </summary>
    /// <remarks>
    /// Provides DateCreated, DateModified, CreatedBy and ModifiedBy properties.
    /// </remarks>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    public abstract class SignedAuditableEntity<TEntity> : AuditableEntity<TEntity>, ISignedCreatable, ISignedModifiable
        where TEntity : SignedAuditableEntity<TEntity>
    {
        public string CreatedBy { get; protected set; }
        public string ModifiedBy { get; protected set; }

        protected SignedAuditableEntity() : base() { }
    }

    /// <summary>
    /// Base class for a signed auditable entity <typeparamref name="TEntity"/> with identifier <typeparamref name="TKey"/>.
    /// </summary>
    /// <remarks>
    /// Provides DateCreated, DateModified, CreatedBy and ModifiedBy properties.
    /// </remarks>
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
