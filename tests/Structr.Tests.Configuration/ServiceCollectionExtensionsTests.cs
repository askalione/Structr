using Microsoft.Extensions.DependencyInjection;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Configuration
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddNavigation()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");

            // Act
            var serviceProvider = new ServiceCollection()
                .AddConfiguration()
                    .AddJson<TestSettings>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsConfigurationServices<TestSettings>();
        }
    }
}
