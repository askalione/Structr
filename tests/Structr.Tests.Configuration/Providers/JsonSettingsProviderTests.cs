using FluentAssertions;
using Structr.Configuration;
using Structr.Configuration.Providers;
using Structr.Tests.Configuration.TestUtils;
using Structr.Tests.Configuration.TestUtils.Extensions;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Configuration.Providers
{
    [Collection("TestSettings")]
    public class JsonSettingsProviderTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new JsonSettingsProvider<TestSettings>(new SettingsProviderOptions(), "SomePath");

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new JsonSettingsProvider<TestSettings>(null, "SomePath");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*options*");
        }

        [Fact]
        public void Ctor_throws_when_options_is_empty()
        {
            // Act
            Action act = () => new JsonSettingsProvider<TestSettings>(new SettingsProviderOptions(), "");

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*path*");
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
        public async Task GetSettings_normal()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_normal),
                ("FilePath", @"""X:\\readme.txt"""));

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.FilePath.Should().Be(@"X:\readme.txt");
        }

        [Fact]
        public async Task GetSettings_alias()
        {
            // Arrange
            var settingsProvider = await GetSettingsProviderAsync(nameof(GetSettings_alias),
                ("SomeOwnerNameAlias", "\"Owner name\""));

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
                ("HelpUrl", "\"support.example.com\""));

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.HelpUrl.Should().Be("support.example.com");
        }

        [Fact]
        public async Task SetSettings_normal()
        {
            // Arrange            
            var settingsProvider = await GetSettingsProviderAsync(nameof(SetSettings_normal));
            var path = settingsProvider.GetPath();
            
            // Act
            settingsProvider.SetSettings(new TestSettings { FilePath = "X:\\readme123.txt" });

            // Assert
            var json = await GetJsonAsync(path);
            json.Should().Contain(@"""FilePath"": ""X:\\readme123.txt"""); // ???
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
            var json = await GetJsonAsync(path);
            json.Should().Contain(@"""SomeOwnerNameAlias"": ""Owner name"""); // ???
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
            var json = await GetJsonAsync(path);
            settings.ApiKey.Should().Be("123abc_qwerty&^");
            json.Should().NotContain("123abc_qwerty&^");
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

        private async Task<string> GetJsonAsync(string fileName)
        {
            return await File.ReadAllTextAsync(fileName);
        }

        private async Task<JsonSettingsProvider<TestSettings>> GetSettingsProviderAsync(string fileName, params (string Name, string Value)[] data)
        {
            var options = new SettingsProviderOptions();
            var path = string.IsNullOrEmpty(fileName) == false ? await GenerateJsonAsync(fileName, data) : "null";
            var settingsProvider = new JsonSettingsProvider<TestSettings>(options, path);
            return settingsProvider;
        }

        private async Task<string> GenerateJsonAsync(string fileName, params (string Name, string Value)[] data)
        {
            fileName = TestDataPath.CombineWithTemp(this.GetType().Name + "+" + fileName + ".json");
            var result = "{" + string.Join(",", data.Select(x => $"\"{x.Name}\": {x.Value}")) + "}";
            await File.WriteAllTextAsync(fileName, result);
            return fileName;
        }
    }
}
