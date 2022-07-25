using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Configuration.Providers
{
    [Collection("Tests with temp files")]
    public class XmlSettingsProviderTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new XmlSettingsProvider<TestSettings>("SomePath", new SettingsProviderOptions());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new XmlSettingsProvider<TestSettings>("SomePath", null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*options*");
        }

        [Fact]
        public void Ctor_throws_when_path_is_empty()
        {
            // Act
            Action act = () => new XmlSettingsProvider<TestSettings>("", new SettingsProviderOptions());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*path*");
        }

        [Fact]
        public async Task GetSettings_normal()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_normal),
                ("FileName", @"readme.txt"));

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.FileName.Should().Be(@"readme.txt");
        }

        [Fact]
        public async Task GetSettings_alias()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_alias),
                ("SomeOwnerNameAlias", "Owner name"));

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.OwnerName.Should().Be("Owner name");
        }

        [Fact]
        public async Task GetSettings_default_if_no_such_field()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_default_if_no_such_field),
                ("SomeOtherProperty", "123"));

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.HelpUrl.Should().Be("help.example.com");
        }

        [Fact]
        public async Task GetSettings_default_if_field_exists()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_default_if_field_exists),
                ("HelpUrl", "support.example.com"));

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.HelpUrl.Should().Be("support.example.com");
        }

        [Fact]
        public async Task GetSettings_doesnt_access_file_when_cache_turned_on()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_doesnt_access_file_when_cache_turned_on),
                ("FileName", @"readme.txt"));
            var path = settingsProvider.GetPath();

            TestSettings settings1 = settingsProvider.GetSettings();
            await TestDataManager.ReplaceFileAsync(path,
                () => TestDataManager.GenerateXmlFileAsync(Path.GetFileNameWithoutExtension(path), ("FileName", @"readme_changed.txt")));

            // Act
            TestSettings settings2 = settingsProvider.GetSettings();

            // Assert
            settings2.FileName.Should().Be(settings1.FileName);
        }

        [Fact]
        public async Task GetSettings_does_access_file_when_cache_turned_off()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderNoCacheAsync(nameof(GetSettings_does_access_file_when_cache_turned_off),
                ("FileName", @"readme.txt"));
            var path = settingsProvider.GetPath();

            TestSettings settings1 = settingsProvider.GetSettings();
            await TestDataManager.ReplaceFileAsync(path,
                () => TestDataManager.GenerateXmlFileAsync(Path.GetFileNameWithoutExtension(path), ("FileName", @"readme_changed.txt")));

            // Act
            TestSettings settings2 = settingsProvider.GetSettings();

            // Assert
            settings2.FileName.Should().NotBe(settings1.FileName);
        }

        [Fact]
        public async Task GetSettings_tracks_modification_despite_cache_turned_on()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_tracks_modification_despite_cache_turned_on),
                ("FileName", @"readme.txt"));
            var fileName = Path.GetFileNameWithoutExtension(settingsProvider.GetPath());

            settingsProvider.GetSettings();

            await TestDataManager.GenerateXmlFileAsync(fileName,
                ("FileName", @"readme_changed.txt"));

            // Act
            var settingsAfterChanges = settingsProvider.GetSettings();

            // Assert
            settingsAfterChanges.FileName.Should().Be(@"readme_changed.txt");
        }

        [Fact]
        public async Task SetSettings_normal()
        {
            // Arrange            
            var settingsProvider = await GetSettingsProviderAsync(nameof(SetSettings_normal));
            var path = settingsProvider.GetPath();

            // Act
            settingsProvider.SetSettings(new TestSettings { FileName = "readme123.txt" });

            // Assert
            var xml = await TestDataManager.GetXmlAsync(path);
            xml.Should().Contain(@"<FileName>readme123.txt</FileName>");
        }

        [Fact]
        public async Task SetSettings_alias()
        {
            // Arrange            
            var settingsProvider = await GetSettingsProviderAsync(nameof(SetSettings_alias));
            var path = settingsProvider.GetPath();

            // Act
            settingsProvider.SetSettings(new TestSettings { OwnerName = "Owner name" });

            // Assert
            var xml = await TestDataManager.GetXmlAsync(path);
            xml.Should().Contain(@"<SomeOwnerNameAlias>Owner name</SomeOwnerNameAlias>");
        }

        [Fact]
        public async Task SetSettings_and_GetSettings_with_encryption()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(SetSettings_and_GetSettings_with_encryption));
            var path = settingsProvider.GetPath();

            // Act
            settingsProvider.SetSettings(new TestSettings { ApiKey = "123abc_qwerty&^" });

            // Assert
            var settings = settingsProvider.GetSettings();
            settings.ApiKey.Should().Be("123abc_qwerty&^");

            var xml = await TestDataManager.GetXmlAsync(path);
            xml.Should().NotContain("123abc_qwerty&^");
        }

        [Fact]
        public async Task SetSettings_throws_when_settings_are_null()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(null);

            // Act
            Action act = () => settingsProvider.SetSettings(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        private async Task<XmlSettingsProvider<TestSettings>> GetSettingsProviderAsync(string fileName, params (string Name, string Value)[] data)
            => await TestDataManager.GetSettingsXmlProviderAsync(this.GetType().Name + "+" + fileName, true, data);

        private async Task<XmlSettingsProvider<TestSettings>> GetSettingsProviderNoCacheAsync(string fileName, params (string Name, string Value)[] data)
            => await TestDataManager.GetSettingsXmlProviderAsync(this.GetType().Name + "+" + fileName, false, data);
    }
}
