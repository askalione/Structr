using System;

namespace Structr.Domain
{
    public interface IModifiable : IAuditable
    {
        DateTime DateModified { get; }
    }
}
