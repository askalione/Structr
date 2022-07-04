using System;

namespace Structr.EntityFrameworkCore
{
    /// <summary>
    /// Delegate to control date and time of creation, modification or deleting of entities.
    /// </summary>
    /// <returns>Date and time of an operation.</returns>
    public delegate DateTime AuditTimestampProvider();
}
