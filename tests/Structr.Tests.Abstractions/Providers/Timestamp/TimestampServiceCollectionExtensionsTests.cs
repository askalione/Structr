using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Abstractions.Providers.Timestamp;
using Xunit;

namespace Structr.Tests.Abstractions.Providers.Timestamp
{
    public class TimestampServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddTimestampProvider_LocalTimestampProvider()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddTimestampProvider<LocalTimestampProvider>()
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ITimestampProvider>()
                .Should().BeOfType<LocalTimestampProvider>();
        }

        [Fact]
        public void AddTimestampProvider_UtcTimestampProvider()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddTimestampProvider<UtcTimestampProvider>()
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ITimestampProvider>()
                .Should().BeOfType<UtcTimestampProvider>();
        }


    }
}
