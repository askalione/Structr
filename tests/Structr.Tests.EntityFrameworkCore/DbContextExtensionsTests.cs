using Structr.EntityFrameworkCore;
using Structr.Tests.EntityFrameworkCore.TestUtils.DataAccess;
using Structr.Tests.EntityFrameworkCore.TestUtils.Domain.Foos;

namespace Structr.Tests.EntityFrameworkCore
{
    public class DbContextExtensionsTests : IDisposable
    {
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