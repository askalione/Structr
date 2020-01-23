using Structr.Abstractions;
using Structr.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structr.Samples.Data.EFCore.Domain.FooAggregate
{
    public class Foo : Entity<Foo, int>, ICreatable, IModifiable, ISoftDeletable
    {
        public EFooType Type { get; private set; }
        public FooDetail Detail { get; private set; } = new FooDetail();

        public ICollection<FooItem> Items { get; private set; } = new HashSet<FooItem>();

        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }
        public DateTime? DateDeleted { get; private set; }

        private Foo() { }

        public Foo(EFooType type, FooDetail detail) : this()
        {
            Ensure.NotNull(detail, nameof(detail));

            Type = type;
            Detail = detail;
        }
    }
}
