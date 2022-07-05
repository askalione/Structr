using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Structr.Abstractions.Providers.Timestamp;
using Structr.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Structr.Tests.EntityFrameworkCore
{
    public class DbContextOptionsExtensionsTests
    {
        private class TestDbContext : DbContext
        {
            // NOTE: Public for tests only.
            public readonly ITimestampProvider TimestampProvider;
            public readonly IPrincipal Principal;

            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
            {
                TimestampProvider = options.GetService<ITimestampProvider>();
                Principal = options.GetService<IPrincipal>();
            }
        }

        [Fact]
        public void GetService()
        {
            // Arrange
            ServiceProvider serviceProvider = new ServiceCollection()
                .AddTimestampProvider<LocalTimestampProvider>()
                .AddScoped<IPrincipal>(provider =>
                {
                    return new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
                        new Claim(ClaimTypes.Name, "User-1")
                    }));
                })
                .AddDbContext<TestDbContext>(options =>
                {
                    options.UseInMemoryDatabase(nameof(TestDbContext));
                })
                .BuildServiceProvider();

            // Act
            TestDbContext dbContext = serviceProvider.GetRequiredService<TestDbContext>();

            // Assert
            dbContext.TimestampProvider.Should().BeOfType<LocalTimestampProvider>();
            dbContext.Principal.Should().BeOfType<ClaimsPrincipal>()
                .Subject.Identity.Should().BeOfType<ClaimsIdentity>()
                .Subject.Name.Should().Be("User-1");
        }
    }
}
