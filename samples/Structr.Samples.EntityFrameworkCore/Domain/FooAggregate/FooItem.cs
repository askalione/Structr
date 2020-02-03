using Structr.Abstractions;
using Structr.Domain;
using System;

namespace Structr.Samples.EntityFrameworkCore.Domain.FooAggregate
{
    public class FooItem : Entity<FooItem, Guid>, ISoftDeletable
    {
        public string Name { get; private set; }

        public DateTime? DateDeleted { get; private set; }

        private FooItem() { }

        public FooItem(string name) : this()
        {
            Ensure.NotEmpty(name, nameof(name));

            Id = SequentialGuid.NewGuid();
            Name = name;
        }
    }
}
