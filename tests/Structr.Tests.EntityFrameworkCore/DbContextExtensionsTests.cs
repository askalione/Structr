using Microsoft.EntityFrameworkCore;
using Structr.Domain;
using Structr.EntityFrameworkCore;

namespace Structr.Tests.EntityFrameworkCore
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
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                Name = name;
            }

            public void Edit(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException(nameof(name));
                }

                Name = name;
            }
        }

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; private set; } = default!;

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                => optionsBuilder
                    .UseInMemoryDatabase(nameof(TestDbContext))
                    .UseInternalServiceProvider(InMemoryFixture.DefaultServiceProvider);
        }

        private TestDbContext _context;

        public DbContextExtensionsTests()
        {
            _context = new TestDbContext();
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