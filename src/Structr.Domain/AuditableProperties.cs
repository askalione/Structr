namespace Structr.Domain
{
    /// <summary>
    /// Defines names for auditable properties.
    /// </summary>
    public static class AuditableProperties
    {
        /// <inheritdoc cref="ICreatable.DateCreated"/>
        public const string DateCreated = nameof(ICreatable.DateCreated);

        /// <inheritdoc cref="ISignedCreatable.CreatedBy"/>
        public const string CreatedBy = nameof(ISignedCreatable.CreatedBy);

        /// <inheritdoc cref="IModifiable.DateModified"/>
        public const string DateModified = nameof(IModifiable.DateModified);

        /// <inheritdoc cref="ISignedModifiable.ModifiedBy"/>
        public const string ModifiedBy = nameof(ISignedModifiable.ModifiedBy);

        /// <inheritdoc cref="ISoftDeletable.DateDeleted"/>
        public const string DateDeleted = nameof(ISoftDeletable.DateDeleted);

        /// <inheritdoc cref="ISignedSoftDeletable.DeletedBy"/>
        public const string DeletedBy = nameof(ISignedSoftDeletable.DeletedBy);
    }
}
