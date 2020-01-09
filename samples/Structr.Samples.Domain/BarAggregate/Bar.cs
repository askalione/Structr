using Structr.Domain;
using Structr.Samples.Domain.FooAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Domain.BarAggregate
{
    public class Bar : Entity<Bar>
    {
        public Guid Id { get; private set; }
        public EBarType Type { get; private set; }

        public Bar(Guid id, EBarType type)
        {
            Id = id;
            Type = type;
        }

        public override bool Equals(Bar other)
        {
            if (other == null)
                return false;

            return Id == other.Id;
        }

        public override bool IsTransient()
        {
            return Id == Guid.Empty;
        }
    }
}
