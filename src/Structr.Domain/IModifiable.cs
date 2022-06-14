using System;

namespace Structr.Domain
{
    /// <summary>
    /// Provides information about an auditable entity modification date.
    /// </summary>
    public interface IModifiable : IAuditable
    {
        /// <summary>
        /// An entity modification date.
        /// </summary>
        DateTime DateModified { get; }
    }
}
