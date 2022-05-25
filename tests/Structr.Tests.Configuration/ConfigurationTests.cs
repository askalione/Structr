using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("TestSettings")]
    public class ConfigurationTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var provider = new JsonSettingsProvider<TestSettings>(new SettingsProviderOptions(), path);
            var options = new ConfigurationOptions<TestSettings>(provider);

            // Act
            var result = new Configuration<TestSettings>(options);

            // Assert
            result.Settings.ShouldBeEquivalentToDefaultSettings();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new Configuration<TestSettings>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
