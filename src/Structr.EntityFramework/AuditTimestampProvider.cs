using System;

namespace Structr.EntityFramework
{
    /// <summary>
    /// Delegate to control date and time creates, modifies or deletes an entity.
    /// </summary>
    /// <returns>Date and time of an operation.</returns>
    public delegate DateTime AuditTimestampProvider();
}
