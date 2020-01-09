using Structr.Domain;
using System;

namespace Structr.Samples.Domain.FooAggregate
{
    public class Foo : Entity<Foo, int>
    {
        public string Name { get; private set; }

        public Foo(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
        }
    }
}
