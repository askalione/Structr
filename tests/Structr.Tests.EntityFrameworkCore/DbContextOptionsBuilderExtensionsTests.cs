using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Structr.EntityFrameworkCore;

namespace Structr.Tests.EntityFrameworkCore
{
    public class DbContextOptionsBuilderExtensionsTests
    {
        private class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        }

        [Fact]
        public void UseLoggerProvider()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options =>
                {
                    options
                        .UseLoggerProvider(NullLoggerProvider.Instance)
                        .UseInMemoryDatabase(nameof(TestDbContext));
                })
                .BuildServiceProvider();

            // Assert
            TestDbContext dbContext = serviceProvider.GetRequiredService<TestDbContext>();
            ILoggerFactory loggerFactory = dbContext.GetService<ILoggerFactory>();            

            ILogger<TestDbContext> logger = loggerFactory.CreateLogger<TestDbContext>();

            // TODO
            //logger.Should().BeOfType<NullLogger<TestDbContext>>();
        }
    }
}
