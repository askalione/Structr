using System;

namespace Structr.Domain
{
    public interface ICreatable : IAuditable
    {
        DateTime DateCreated { get; }
    }
}
