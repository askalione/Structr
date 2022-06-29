using Structr.Abstractions;
using Structr.Abstractions.Providers.SequentialGuid;
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

            var sequentialGuidProvider = new SequentialGuidProvider(Guid.NewGuid, () => DateTime.UtcNow);
            Id = sequentialGuidProvider.GetSequentialGuid();
            Name = name;
        }
    }
}
