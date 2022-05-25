using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("TestSettings")]
    public class ConfigurationOptionsTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var provider = new JsonSettingsProvider<TestSettings>(new SettingsProviderOptions(), path);

            // Act
            var result = new ConfigurationOptions<TestSettings>(provider);

            // Assert
            result.Should().NotBeNull();
            result.Provider.Should().BeEquivalentTo(provider);
        }

        [Fact]
        public void Ctor_throws_when_provider_is_null()
        {
            // Act
            Action act = () => new ConfigurationOptions<TestSettings>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
