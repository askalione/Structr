namespace Structr.Domain
{
    /// <summary>
    /// Provides information about who deleted an auditable entity and when.
    /// </summary>
    public interface ISignedSoftDeletable : ISoftDeletable
    {
        /// <summary>
        /// Defines who deleted the entity.
        /// </summary>
        string DeletedBy { get; }
    }
}
