using System;

namespace Structr.Domain
{
    public interface ISoftDeletable : IAuditable
    {
        DateTime? DateDeleted { get; }
    }
}
