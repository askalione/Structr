using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Tests.Configuration.TestUtils;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("Tests with temp files")]
    public class ConfiguratorTests
    {
        [Fact]
        public async Task Ctor()
        {
            // Arrange
            var provider = await TestDataManager.GetSettingsJsonProviderAsync(nameof(ConfigurationTests) + nameof(Ctor), true,
                ("FilePath", @"""X:\\readme.txt"""));
            var options = new ConfigurationOptions<TestSettings>(provider);

            // Act
            Action act = () => new Configurator<TestSettings>(options);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_are_null()
        {
            // Act
            Action act = () => new Configurator<TestSettings>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task Configure()
        {
            // Arrange
            var path = await TestDataManager.GenerateJsonFileAsync(nameof(ConfigurationTests) + nameof(Configure),
                ("FilePath", @"""X:\\SomeOtherFile.txt"""));
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
            var serviceProvider = new ServiceCollection()
                .AddConfiguration()
                .AddJson<TestSettings>("Some path")
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
