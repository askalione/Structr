using Effort.Provider;
using Structr.Abstractions;
using Structr.Domain;
using Structr.EntityFramework;
using System.Data.Common;
using System.Data.Entity;

namespace Structr.Tests.EntityFramework
{
    public class DbContextExtensionsTests : IDisposable
    {
        private class Foo : Entity<Foo, int>, ISignedCreatable, ISignedModifiable, ISignedSoftDeletable
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

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; set; } = default!;

            public TestDbContext(DbConnection dbConnection) : base(dbConnection, true) { }
        }

        private TestDbContext _context;

        public DbContextExtensionsTests()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new TestDbContext(connection);
        }

        [Fact]
        public void Audit_create()
        {
            // Arrange
            var foo = new Foo("Foo 1");
            var dateCreated = DateTime.Now;
            _context.Foos.Add(foo);

            // Act
            _context.Audit(() => dateCreated, () => "peter@parker.name");

            // Assert
            foo.DateCreated.Should().Be(dateCreated);
            foo.CreatedBy.Should().Be("peter@parker.name");
        }

        [Fact]
        public void Audit_modify()
        {
            // Arrange
            var foo = new Foo("Foo 1");
            var dateCreated = DateTime.Now;
            _context.Foos.Add(foo);
            _context.Audit(() => dateCreated, () => "peter@parker.name");

            foo.Edit("Foo 2");
            var dateModified = DateTime.Now;

            // Act
            _context.Audit(() => dateModified, () => "peter@parker.name");

            // Assert
            foo.DateModified.Should().Be(dateModified);
            foo.ModifiedBy.Should().Be("peter@parker.name");
        }

        [Fact]
        public void Audit_delete()
        {
            // Arrange
            var foo = new Foo("Foo 1");
            var dateCreated = DateTime.Now;
            _context.Foos.Add(foo);
            _context.Audit(() => dateCreated, () => "peter@parker.name");
            _context.SaveChanges();

            _context.Foos.Remove(foo);
            var dateDeleted = DateTime.Now;

            // Act
            _context.Audit(() => dateDeleted, () => "peter@parker.name");

            // Assert
            foo.DateDeleted.Should().Be(dateDeleted);
            foo.DeletedBy.Should().Be("peter@parker.name");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
