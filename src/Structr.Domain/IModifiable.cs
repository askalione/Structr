using System;

namespace Structr.Domain
{
    public interface IModifiable
    {
        DateTime DateModified { get; }
    }
}
