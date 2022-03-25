using Structr.Domain;
using System;

namespace Structr.Samples.Domain.BazAggregate
{
    public class Baz : Entity<Baz, BazId>
    {
        public string Name { get; }

        public Baz(BazId id, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
        }

        // NOTE: IsTransient() always return false because PK is Enum
        // and all entities with type Baz already stored in DB for example.
        public override bool IsTransient()
            => false;
    }
}
