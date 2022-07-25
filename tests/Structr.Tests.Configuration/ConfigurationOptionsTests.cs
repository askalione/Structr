using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("Tests with temp files")]
    public class ConfigurationOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var provider = new JsonSettingsProvider<TestSettings>("Some path", new SettingsProviderOptions());

            // Act
            var result = new ConfigurationOptions<TestSettings>(provider);

            // Assert
            result.Provider.Should().Be(provider);
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
