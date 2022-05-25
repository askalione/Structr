using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Tests.Configuration.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("TestSettings")]
    public class ConfiguratorTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var serviceProvider = new ServiceCollection()
                .AddConfiguration()
                .AddJson<TestSettings>(path)
                .Services
                .BuildServiceProvider();

            // Act
            var result = new Configurator<TestSettings>(serviceProvider);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Ctor_throws_when_serviceProvider_is_null()
        {
            // Act
            Action act = () => new Configurator<TestSettings>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_if_configuration_service_is_not_configured()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddConfiguration()
                .Services
                .BuildServiceProvider();

            // Act
            Action act = () => new Configurator<TestSettings>(serviceProvider);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Configure()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var serviceProvider = new ServiceCollection()
                .AddConfiguration()
                .AddJson<TestSettings>(path)
                .Services
                .BuildServiceProvider();
            var configurator = serviceProvider.GetRequiredService<IConfigurator<TestSettings>>();

            // Act
            configurator.Configure(settings => settings.FilePath = "X:\\readme.txt");

            // Assert
            var configuration = serviceProvider.GetRequiredService<IConfiguration<TestSettings>>();
            var settings = configuration.Settings;
            settings.FilePath.Should().Be("X:\\readme.txt");
        }

        [Fact]
        public void Configure_throws_when_changes_are_null()
        {
            // Arrange
            var path = TestDataPath.Combine("settings.json");
            var serviceProvider = new ServiceCollection()
                .AddConfiguration()
                .AddJson<TestSettings>(path)
                .Services
                .BuildServiceProvider();
            var configurator = serviceProvider.GetRequiredService<IConfigurator<TestSettings>>();

            // Act
            Action act = () => configurator.Configure(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
