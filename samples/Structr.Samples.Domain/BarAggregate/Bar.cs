using Structr.Domain;
using System;

namespace Structr.Samples.Domain.BarAggregate
{
    public class Bar : Entity<Bar, Guid>
    {
        public EBarType Type { get; private set; }

        public Bar(Guid id, EBarType type)
        {
            Id = id;
            Type = type;
        }
    }
}
