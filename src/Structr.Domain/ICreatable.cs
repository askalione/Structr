using System;

namespace Structr.Domain
{
    /// <summary>
    /// Provides information about an auditable entity creation date.
    /// </summary>
    public interface ICreatable : IAuditable
    {
        /// <summary>
        /// Defines an entity creation date.
        /// </summary>
        DateTime DateCreated { get; }
    }
}
