namespace Structr.Domain
{
    /// <summary>
    /// Provides information about who modified an auditable entity and when.
    /// </summary>
    public interface ISignedModifiable : IModifiable
    {
        /// <summary>
        /// Defines who modified the entity.
        /// </summary>
        string ModifiedBy { get; }
    }
}
