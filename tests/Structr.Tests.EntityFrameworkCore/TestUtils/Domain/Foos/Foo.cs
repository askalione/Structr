using Structr.Abstractions;
using Structr.Domain;

namespace Structr.Tests.EntityFrameworkCore.TestUtils.Domain.Foos
{
    public class Foo : Entity<Foo, int>, ISignedCreatable, ISignedModifiable, ISignedSoftDeletable
    {
        public string Name { get; private set; } = default!;

        public DateTime DateCreated { get; private set; }
        public string CreatedBy { get; private set; } = default!;

        public DateTime DateModified { get; private set; }
        public string ModifiedBy { get; private set; } = default!;

        public DateTime? DateDeleted { get; private set; }
        public string? DeletedBy { get; private set; }

        private Foo() : base() { }

        public Foo(string name) : this()
        {
            Ensure.NotNull(name, nameof(name));

            Name = name;
        }

        public void Edit(string name)
        {
            Ensure.NotNull(name, nameof(name));

            Name = name;
        }
    }
}
