namespace Structr.EntityFramework
{
    /// <summary>
    /// Delegate to control who creates, modifies or deletes an entity.
    /// </summary>
    /// <returns>Name or identifier.</returns>
    public delegate string AuditSignProvider();
}
