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
        private readonly IEnumerable<Foo> _expected;

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
                var list = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                _context.Foos.AddRange(list.Select(x => new Foo { Name = x }));
                _context.SaveChanges();
            }

            _expected = _context.Foos.Skip(5).ToList();
        }

        [Fact]
        public void ToPagedList()
        {
            // Arrange
            IQueryable<Foo> source = _context.Foos;

            // Act
            PagedList<Foo> result = source.ToPagedList(2, 5);

            // Assert
            result.Should().BeEquivalentTo(_expected);
        }

        [Theory]
        [InlineData(null)]
        public void ToPagedList_throws_when_source_is_null(IQueryable<string> source)
        {
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
            result.Should().BeEquivalentTo(_expected);
        }

        [Theory]
        [InlineData(null)]
        public async Task ToPagedListAsync_throws_when_source_is_null(IQueryable<string> source)
        {
            // Act
            Func<Task> act = async () => await source.ToPagedListAsync(2, 5);

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentNullException>();
        }
    }
}
