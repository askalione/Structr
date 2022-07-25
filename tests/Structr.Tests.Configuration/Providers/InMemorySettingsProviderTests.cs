using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Configuration.Providers
{
    public class InMemorySettingsProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new InMemorySettingsProvider<TestSettings>(new TestSettings(), new SettingsProviderOptions());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_settings_is_null()
        {
            // Act
            Action act = () => new InMemorySettingsProvider<TestSettings>(null, new SettingsProviderOptions());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*settings*");
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new InMemorySettingsProvider<TestSettings>(new TestSettings(), null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*options*");
        }

        [Fact]
        public void GetSettings()
        {
            // Arrange
            var testSettings = new TestSettings { FileName = "readme.txt" };
            var settingsProvider = GetSettingsProvider(testSettings);
            testSettings.FileName = "readme123.txt";

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.Should().Be(testSettings);
        }

        [Fact]
        public void SetSettings()
        {
            // Arrange            
            var testSettings = new TestSettings { FileName = "readme.txt" };
            var settingsProvider = GetSettingsProvider(testSettings);
            var testSettings2 = new TestSettings { FileName = "readme123.txt" };

            // Act
            settingsProvider.SetSettings(testSettings2);

            // Assert
            TestSettings settings = settingsProvider.GetSettings();
            settings.Should().Be(testSettings2);
        }

        [Fact]
        public void SetSettings_throws_when_settings_are_null()
        {
            // Arrange
            var settingsProvider = GetSettingsProvider(new TestSettings());

            // Act
            Action act = () => settingsProvider.SetSettings(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        private InMemorySettingsProvider<TestSettings> GetSettingsProvider(TestSettings settings, SettingsProviderOptions options = null)
            => new InMemorySettingsProvider<TestSettings>(settings, options ?? new SettingsProviderOptions());
    }
}
