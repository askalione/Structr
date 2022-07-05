using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Structr.EntityFrameworkCore;

namespace Structr.Tests.EntityFrameworkCore
{
    public class DbContextOptionsBuilderExtensionsTests
    {
        private class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        }

        private class FakeLoggerProvider : ILoggerProvider
        {
            private readonly List<string> _logs;

            public FakeLoggerProvider(List<string> logs)
                => _logs = logs;

            public ILogger CreateLogger(string categoryName)
                => new FakeLogger(_logs);

            public void Dispose() { }
        }

        private class FakeLogger : ILogger
        {
            private readonly List<string> _logs;

            public FakeLogger(List<string> logs)
                => _logs = logs;

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotSupportedException();
            }

            public bool IsEnabled(LogLevel logLevel)
                => true;

            public void Log<TState>(LogLevel logLevel,
                EventId eventId,
                TState state,
                Exception? exception,
                Func<TState, Exception?, string> formatter)
            {
                _logs.Add(formatter(state, exception));
            }
        }

        [Fact]
        public void UseLoggerProvider()
        {
            // Arrange
            List<string> logs = new List<string>();
            var loggerProvider = new FakeLoggerProvider(logs);

            // Act
            var serviceProvider = new ServiceCollection()
                .AddDbContext<TestDbContext>(options =>
                {
                    options
                        .UseLoggerProvider(loggerProvider)
                        .UseInMemoryDatabase(nameof(TestDbContext));
                })
                .BuildServiceProvider();

            // Assert
            TestDbContext dbContext = serviceProvider.GetRequiredService<TestDbContext>();
#pragma warning disable EF1001 // Internal EF Core API usage.
            ILoggerFactory loggerFactory = dbContext.GetService<IDbContextServices>().ContextOptions!
                .FindExtension<CoreOptionsExtension>()!
                .LoggerFactory!;
#pragma warning restore EF1001 // Internal EF Core API usage.
            ILogger logger = loggerFactory.CreateLogger(typeof(TestDbContext));
            logger.LogError("Some error");

            logs.LastOrDefault().Should().Be("Some error");
        }
    }
}
