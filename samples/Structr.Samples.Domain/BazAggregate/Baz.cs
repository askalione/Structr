using Structr.Domain;
using System;

namespace Structr.Samples.Domain.BazAggregate
{
    public class Baz : Entity<Baz, EBaz>
    {
        public string Name { get; }

        public Baz(EBaz id, string name)
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
