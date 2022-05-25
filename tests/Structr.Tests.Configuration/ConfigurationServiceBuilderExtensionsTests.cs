using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("TestSettings")]
    public class ConfigurationServiceBuilderExtensionsTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void AddProvider_json()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var provider = new JsonSettingsProvider<TestSettings>(new SettingsProviderOptions(), path);
            var configurationBuilder = new ServiceCollection()
                .AddConfiguration();

            // Act
            var serviceProvider = configurationBuilder
                .AddProvider(provider)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsConfigurationServices<TestSettings>();
        }

        [Fact]
        public void AddProvider_xml()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.xml");
            var provider = new XmlSettingsProvider<TestSettings>(new SettingsProviderOptions(), path);
            var configurationBuilder = new ServiceCollection()
                .AddConfiguration();

            // Act
            var serviceProvider = configurationBuilder
                .AddProvider(provider)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsConfigurationServices<TestSettings>();
        }
    }
}
