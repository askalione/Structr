using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("Tests with temp files")]
    public class ConfigurationServiceBuilderExtensionsTests
    {
        [Fact]
        public void AddProvider_json()
        {
            // Arrange
            var provider = new JsonSettingsProvider<TestSettings>("Some path", new SettingsProviderOptions());
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
            var provider = new XmlSettingsProvider<TestSettings>("Some path", new SettingsProviderOptions());
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
        public void AddProvider_in_memory()
        {
            // Arrange
            var provider = new InMemorySettingsProvider<TestSettings>(new TestSettings(), new SettingsProviderOptions());
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
