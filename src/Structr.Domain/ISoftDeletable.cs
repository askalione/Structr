using System;

namespace Structr.Domain
{
    public interface ISoftDeletable : IUndeletable
    {
        DateTime? DateDeleted { get; }
    }
}
