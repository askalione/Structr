using System;

namespace Structr.Domain
{
    /// <summary>
    /// Provides information about an auditable entity deletion date.
    /// </summary>
    public interface ISoftDeletable : IUndeletable
    {
        /// <summary>
        /// An entity deletion date.
        /// </summary>
        DateTime? DateDeleted { get; }
    }
}
