using Structr.Domain;
using System;

namespace Structr.Samples.Domain.BarAggregate
{
    public class Bar : Entity<Bar, Guid>
    {
        public BarType Type { get; private set; }

        public Bar(Guid id, BarType type)
        {
            Id = id;
            Type = type;
        }
    }
}
