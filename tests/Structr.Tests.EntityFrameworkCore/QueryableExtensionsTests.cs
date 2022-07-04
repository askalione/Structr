using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Structr.Collections;
using Structr.Domain;
using Structr.EntityFrameworkCore;


namespace Structr.Tests.EntityFrameworkCore
{
    public class QueryableExtensionsTests
    {
        private class Foo : Entity<Foo, int>
        {
            public string Name { get; set; } = default!;
        }

        private class TestDbContext : DbContext
        {
            public DbSet<Foo> Foos { get; private set; } = default!;

            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        }

        private readonly TestDbContext _context;

        public QueryableExtensionsTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options =>
                {
                    options.UseInMemoryDatabase(nameof(TestDbContext));
                })
                .BuildServiceProvider();

            _context = serviceProvider.GetRequiredService<TestDbContext>();

            if (_context.Foos.Any() == false)
            {
                var list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                _context.Foos.AddRange(list.Select(x => new Foo { Name = x }));
                _context.SaveChanges();
            }
        }

        [Fact]
        public void ToPagedList()
        {
            // Arrange
            IQueryable<Foo> source = _context.Foos;

            // Act
            PagedList<Foo> result = source.ToPagedList(2, 5);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string> { "5", "6", "7", "8", "9" });
        }

        [Fact]
        public void ToPagedList_throws_when_source_is_null()
        {
            // Arrange
            IQueryable<string> source = null!;

            // Act
            Action act = () => source.ToPagedList(2, 5);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task ToPagedListAsync()
        {
            // Arrange
            IQueryable<Foo> source = _context.Foos;

            // Act
            PagedList<Foo> result = await source.ToPagedListAsync(2, 5);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo(new List<string> { "5", "6", "7", "8", "9" });
        }

        [Fact]
        public async Task ToPagedListAsync_throws_when_source_is_null()
        {
            // Arrange
            IQueryable<string> source = null!;

            // Act
            Func<Task> act = async () => await source.ToPagedListAsync(2, 5);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}
