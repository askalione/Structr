using Microsoft.EntityFrameworkCore;
using Structr.Tests.EntityFrameworkCore.TestUtils.Domain.Foos;

namespace Structr.Tests.EntityFrameworkCore.TestUtils.DataAccess
{
    public class TestDbContext : DbContext
    {
        public DbSet<Foo> Foos { get; private set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseInMemoryDatabase(nameof(TestDbContext))
                .UseInternalServiceProvider(InMemoryFixture.DefaultServiceProvider);
    }
}
