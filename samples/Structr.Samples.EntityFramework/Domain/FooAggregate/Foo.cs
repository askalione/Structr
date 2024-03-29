using Structr.Abstractions;
using Structr.Domain;
using System;
using System.Collections.Generic;

namespace Structr.Samples.EntityFrameworkCore.Domain.FooAggregate
{
    public class Foo : Entity<Foo, int>, ICreatable, ISignedModifiable, ISoftDeletable
    {
        public FooType Type { get; private set; }
        public FooDetail Detail { get; private set; }

        public virtual ICollection<FooItem> Items { get; private set; } = new HashSet<FooItem>();

        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }
        public string ModifiedBy { get; private set; }
        public DateTime? DateDeleted { get; private set; }

        private Foo() { }

        public Foo(FooType type, FooDetail detail) : this()
        {
            Ensure.NotNull(detail, nameof(detail));

            Type = type;
            Detail = detail;
        }
    }
}
