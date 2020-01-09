using System;

namespace Structr.Domain
{
    public interface ISoftDeletable
    {
        DateTime? DateDeleted { get; }
    }
}
