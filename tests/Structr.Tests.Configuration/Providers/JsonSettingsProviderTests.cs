using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using System;
using Xunit;

namespace Structr.Tests.Configuration.Providers
{
    [Collection("TestSettings")]
    public class JsonSettingsProviderTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var options = new SettingsProviderOptions();
            var path = TestDataPath.Combine("settings.json");

            // Act
            var result = new JsonSettingsProvider<TestSettings>(options, path);

            // Assert
            result.Should().NotBeNull();
            result.GetSettings().ShouldBeEquivalentToDefaultSettings();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");

            // Act
            Action act = () => new JsonSettingsProvider<TestSettings>(null, path);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_path_is_null()
        {
            // Arrange
            var options = new SettingsProviderOptions();

            // Act
            Action act = () => new JsonSettingsProvider<TestSettings>(options, null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetSettings()
        {
            // Arrange
            var options = new SettingsProviderOptions();
            var path = TestDataPath.Combine("settings.json");
            var serringsProvider = new JsonSettingsProvider<TestSettings>(options, path);

            // Act
            var settings = serringsProvider.GetSettings();

            // Assert
            settings.ShouldBeEquivalentToDefaultSettings();
        }

        [Fact]
        public void SetSettings()
        {
            // Arrange            
            var options = new SettingsProviderOptions();
            var path = TestDataPath.Combine("settings.json");
            var serringsProvider = new JsonSettingsProvider<TestSettings>(options, path);

            // Act
            serringsProvider.SetSettings(new TestSettings { FilePath = "X:\\readme.txt" });

            // Assert
            var settings = serringsProvider.GetSettings();
            settings.FilePath.Should().Be("X:\\readme.txt");
        }

        [Fact]
        public void SetSettings_throws_when_settings_are_null()
        {
            // Arrange
            var options = new SettingsProviderOptions();
            var path = TestDataPath.Combine("settings.json");
            var serringsProvider = new JsonSettingsProvider<TestSettings>(options, path);

            // Act
            Action act = () => serringsProvider.SetSettings(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
