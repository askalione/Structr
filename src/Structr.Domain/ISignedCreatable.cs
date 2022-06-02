namespace Structr.Domain
{
    /// <summary>
    /// Provides information about who created an auditable entity and when.
    /// </summary>
    public interface ISignedCreatable : ICreatable
    {
        /// <summary>
        /// Defines who created the entity.
        /// </summary>
        string CreatedBy { get; }
    }
}
