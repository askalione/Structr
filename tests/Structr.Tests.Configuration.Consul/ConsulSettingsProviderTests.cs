using Consul;
using FluentAssertions;
using Moq;
using Structr.Configuration;
using Structr.Configuration.Consul;
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

        private static Mock<IConsulClient> CreateConsulClientMock(string key = "TestSettings", params (string Name, string Value)[] data)
        {
            Mock<IConsulClient> consulClientMock = new Mock<IConsulClient>();

            consulClientMock.Setup(x => x.KV.Get(key, It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new QueryResult<KVPair>
                {
                    Response = new KVPair(key)
                    {
                        Value = data != null
                            ? Encoding.UTF8.GetBytes("{" + string.Join(",", data.Select(x => $"\"{x.Name}\":\"{x.Value}\"")) + "}")
                            : null
                    }
                }));
            consulClientMock.Setup(x => x.KV.Put(It.IsAny<KVPair>(), It.IsAny<CancellationToken>()));

            return consulClientMock;
        }

        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new ConsulSettingsProvider<TestSettings>("TestSettings",
                CreateConsulClientMock().Object,
                new SettingsProviderOptions());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new ConsulSettingsProvider<TestSettings>("TestSettings",
                CreateConsulClientMock().Object,
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
                CreateConsulClientMock().Object,
                new SettingsProviderOptions());

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*key*");
        }

        [Fact]
        public void GetSettings()
        {
            // Arrange
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock(key: "TestSettings", ("Name", "Consul"));
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
                new SettingsProviderOptions());

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.Name.Should().Be("Consul");
            consulClientMock.Verify(x => x.KV.Get("TestSettings", It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public void GetSettings_alias()
        {
            // Arrange
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock(key: "TestSettings", ("client_id", "12345"));
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
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
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock(key: "TestSettings", ("prop", "test"));
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
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
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock(key: "TestSettings", ("BaseUrl", "api.v1.example.com"));
            var settingsProvider = new ConsulSettingsProvider<TestSettings>("TestSettings",
                consulClientMock.Object,
                new SettingsProviderOptions());

            // Act
            var settings = settingsProvider.GetSettings();

            // Assert
            settings.BaseUrl.Should().Be("api.v1.example.com");
        }

        [Fact]
        public void SetSettings()
        {
            // Arrange            
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock();
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
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock();
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
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock();
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
            Mock<IConsulClient> consulClientMock = CreateConsulClientMock();
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
