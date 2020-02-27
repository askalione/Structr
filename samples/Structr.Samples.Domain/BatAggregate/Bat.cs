using Structr.Domain;
using System;

namespace Structr.Samples.Domain.BatAggregate
{
    public class Bat : Entity<Bat, BatId>
    {
        public string Name { get; }

        public Bat(BatId id, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
        }
    }
}
