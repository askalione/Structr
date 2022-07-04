using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Abstractions.Providers.SequentialGuid;
using Xunit;

namespace Structr.Tests.Abstractions.Providers.SequentialGuid
{
    public class SequentialGuidServiceCollectionExtsnsionsTests
    {
        [Fact]
        public void AddSequentialGuid()
        {
            // Act
            var serviceProvider = new ServiceCollection()
                .AddSequentialGuidProvider()
                .BuildServiceProvider();

            // Assert
            serviceProvider.GetService<ISequentialGuidProvider>()
                .Should().BeOfType<SequentialGuidProvider>();
        }
    }
}
