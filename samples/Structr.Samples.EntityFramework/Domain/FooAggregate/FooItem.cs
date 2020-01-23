using Structr.Abstractions;
using Structr.Domain;
using System;

namespace Structr.Samples.EntityFrameworkCore.Domain.FooAggregate
{
    public class FooItem : Entity<FooItem, Guid>
    {
        public int FooId { get; private set; }
        public string Name { get; private set; }

        private FooItem() { }

        public FooItem(int fooId, string name) : this()
        {
            Ensure.NotEmpty(name, nameof(name));

            Id = SequentialGuid.NewGuid();
            FooId = fooId;
            Name = name;
        }
    }
}
