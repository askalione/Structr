using Consul;
using FluentAssertions;
using Moq;
using Structr.Configuration;
using Structr.Configuration.Consul;
using System.IO;
using System.Text;
using Xunit;

namespace Structr.Tests.Configuration.Consul
{
    public class ConsulSettingsProviderTests
    {
        class TestSettings
        {
            public string Name { get; set; } = default!;

            [Option(Alias = "client_id")]
            public string ClientId { get; set; } = default!;

            [Option(EncryptionPassphrase = "abcdef")]
            public string ClientSecret { get; set; } = default!;

            [Option(DefaultValue = "api.example.com")]
            public string BaseUrl { get; set; } = default!;
        }

        class ConsulClientMock
        {
            private readonly string _key;

            public readonly Mock<IConsulClient> Mock;
            public IConsulClient Client => Mock.Object;

            public ConsulClientMock(string key = "TestSettings", params (string Name, string Value)[] data)
            {
                _key = key;
                Mock = new Mock<IConsulClient>();

                SetData(data);
            }

            public void SetData(params (string Name, string Value)[]? data)
            {
                Mock.Setup(x => x.KV.Get(_key, It.IsAny<CancellationToken>()))
                   .Returns(Task.FromResult(new QueryResult<KVPair>
                   {
                       Response = new KVPair(_key)
                       {
                           Value = data != null
                               ? Encoding.UTF8.GetBytes("{" + string.Join(",", data.Select(x => $"\"{x.Name}\":\"{x.Value}\"")) + "}")
                               : null
                       }
                   }));
            }
        }

        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new ConsulSettingsProvider<TestSettings>("TestSettings",
                new ConsulClientMock().Client,
                new SettingsProviderOptions());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new ConsulSettingsProvider<TestSettings>("TestSettings",
                new ConsulClientMock().Client,
                null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*options*");
        }

        [Fact]
        public void Ctor_throws_when_consulClient_is_null()
        {
            // Act
            Action act = () => new ConsulSettingsProvider<TestSettings>("TestSettings",
                null,
                new SettingsProviderOptions());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*consulClient*");
        }


        [Fact]
        public void Ctor_throws_when_key_is_empty()
        {
            // Act
            Action act = () => new ConsulSettingsProvider<TestSettings>("",
                new ConsulClientMock().Client,
                new SettingsProviderOptions());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*key*");
        }

        [Fact]
        public void GetSettings()
        {
            // Arrange
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                new ConsulClientMock(data: ("Name", "Consul")).Client,
                new SettingsProviderOptions());

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.Name.Should().Be("Consul");
        }

        [Fact]
        public void GetSettings_alias()
        {
            // Arrange 
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                new ConsulClientMock(data: ("client_id", "12345")).Client,
                new SettingsProviderOptions());

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.ClientId.Should().Be("12345");
        }

        [Fact]
        public void GetSettings_default_if_no_such_field()
        {
            // Arrange
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                new ConsulClientMock(data: ("prop", "test")).Client,
                new SettingsProviderOptions());

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.BaseUrl.Should().Be("api.example.com");
        }

        [Fact]
        public void GetSettings_default_if_field_exists()
        {
            // Arrange
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                new ConsulClientMock(data: ("BaseUrl", "api.v1.example.com")).Client,
                new SettingsProviderOptions());

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.BaseUrl.Should().Be("api.v1.example.com");
        }

        [Fact]
        public void GetSettings_doesnt_access_Consul_when_cache_turned_on()
        {
            // Arrange
            ConsulClientMock consulClientMock = new ConsulClientMock(data: ("Name", "Consul"));
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Client,
                new SettingsProviderOptions { Cache = true });

            settingsProvider.GetSettings();
            consulClientMock.SetData(("Name", "SomeOtherName"));

            // Act
            TestSettings settings2 = settingsProvider.GetSettings();

            // Assert
            settings2.Name.Should().Be("Consul");
        }

        [Fact]
        public void GetSettings_does_access_Consul_when_cache_turned_off()
        {
            // Arrange
            ConsulClientMock consulClientMock = new ConsulClientMock(data: ("Name", "Consul"));
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Client,
                new SettingsProviderOptions { Cache = false });

            settingsProvider.GetSettings();
            consulClientMock.SetData(("Name", "SomeOtherName"));

            // Act
            TestSettings settings2 = settingsProvider.GetSettings();

            // Assert
            settings2.Name.Should().Be("SomeOtherName");
        }

        [Fact]
        public void SetSettings()
        {
            // Arrange            
            Mock<IConsulClient> consulClientMock = new ConsulClientMock().Mock;
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
                new SettingsProviderOptions());

            // Act
            settingsProvider.SetSettings(new TestSettings { Name = "Consul" });

            // Assert
            consulClientMock.Verify(x =>
                x.KV.Put(It.Is<KVPair>(x => x.Key == "TestSettings" && Encoding.UTF8.GetString(x.Value).Contains("\"Name\":\"Consul\"")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void SetSettings_alias()
        {
            // Arrange            
            Mock<IConsulClient> consulClientMock = new ConsulClientMock().Mock;
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
                new SettingsProviderOptions());

            // Act
            settingsProvider.SetSettings(new TestSettings { ClientId = "12345" });

            // Assert
            consulClientMock.Verify(x =>
                x.KV.Put(It.Is<KVPair>(x => x.Key == "TestSettings" && Encoding.UTF8.GetString(x.Value).Contains("\"client_id\":\"12345\"")), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void SetSettings_encryption()
        {
            // Arrange
            Mock<IConsulClient> consulClientMock = new ConsulClientMock().Mock;
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
                new SettingsProviderOptions());

            // Act
            settingsProvider.SetSettings(new TestSettings { ClientSecret = "secret" });

            // Assert
            consulClientMock.Verify(x =>
                 x.KV.Put(It.Is<KVPair>(x => x.Key == "TestSettings" && Encoding.UTF8.GetString(x.Value).Contains("\"ClientSecret\":\"secret\"") == false), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void SetSettings_throws_when_settings_are_null()
        {
            // Arrange
            Mock<IConsulClient> consulClientMock = new ConsulClientMock().Mock;
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
                new SettingsProviderOptions());

            // Act
            Action act = () => settingsProvider.SetSettings(null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
