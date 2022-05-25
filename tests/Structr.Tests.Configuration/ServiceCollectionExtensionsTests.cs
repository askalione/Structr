using Microsoft.Extensions.DependencyInjection;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("TestSettings")]
    public class ServiceCollectionExtensionsTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void AddConfiguration_json()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var configurationBuilder = new ServiceCollection()
                .AddConfiguration();

            // Act
            var serviceProvider = configurationBuilder
                .AddJson<TestSettings>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsConfigurationServices<TestSettings>();
        }

        [Fact]
        public void AddConfiguration_xml()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.xml");
            var configurationBuilder = new ServiceCollection()
                .AddConfiguration();

            // Act
            var serviceProvider = configurationBuilder
                .AddXml<TestSettings>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsConfigurationServices<TestSettings>();
        }
    }
}
