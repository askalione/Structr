using Structr.Abstractions;
using Structr.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Structr.Samples.Data.EFCore.Domain.FooAggregate
{
    public class FooItem : Entity<FooItem, Guid>
    {
        public string Name { get; private set; }

        private FooItem() { }

        public FooItem(string name) : this()
        {
            Ensure.NotEmpty(name, nameof(name));

            Id = SequentialGuid.NewGuid();
            Name = name;
        }
    }
}
