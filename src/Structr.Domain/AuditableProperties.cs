namespace Structr.Domain
{
    public static class AuditableProperties
    {
        public const string DateCreated = nameof(ICreatable.DateCreated);
        public const string CreatedBy = nameof(ISignedCreatable.CreatedBy);
        public const string DateModified = nameof(IModifiable.DateModified);
        public const string ModifiedBy = nameof(ISignedModifiable.ModifiedBy);
        public const string DateDeleted = nameof(ISoftDeletable.DateDeleted);
        public const string DeletedBy = nameof(ISignedSoftDeletable.DeletedBy);
    }
}
