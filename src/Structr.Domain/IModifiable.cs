using System;

namespace Structr.Domain
{
    /// <summary>
    /// Provides information about an auditable entity modification date.
    /// </summary>
    public interface IModifiable : IAuditable
    {
        /// <summary>
        /// Defines an entity modification date.
        /// </summary>
        DateTime DateModified { get; }
    }
}
