using Effort.Provider;
using Structr.Domain;
using Structr.EntityFramework;
using System.Data.Common;
using System.Data.Entity;

namespace Structr.Tests.EntityFramework
{
    public class DbSetExtensionsTests : IDisposable
    {
        public class Foo : Entity<Foo, int>
        {
            public string Name { get; set; } = default!;
        }

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; set; } = default!;

            public TestDbContext(DbConnection dbConnection) : base(dbConnection, true) { }
        }

        private TestDbContext _context;

        public DbSetExtensionsTests()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new TestDbContext(connection);

            _context.Foos.Add(new Foo { Name = "Foo 1" });
            _context.SaveChanges();
        }

        [Fact]
        public void GetContext()
        {
            // Act
            DbContext result = _context.Foos.GetContext();

            // Assert
            result.Should().Be(_context);
        }

        [Fact]
        public void Update()
        {
            // Arrange
            Foo foo = _context.Foos.First();

            // Act
            _context.Foos.Update(foo);

            // Assert
            _context.Entry(foo).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        public void Update_throws_then_entity_is_null()
        {
            // Act
            Action act = () => _context.Foos.Update(null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
