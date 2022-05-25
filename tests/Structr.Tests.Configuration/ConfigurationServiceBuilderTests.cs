using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using Structr.Tests.Configuration.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("TestSettings")]
    public class ConfigurationServiceBuilderTests : IClassFixture<TestSettingsFixture>
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = new ConfigurationServiceBuilder(services);

            // Assert
            result.Should().NotBeNull();
            result.Services.Should().BeEquivalentTo(services);
        }

        [Fact]
        public void Ctor_throws_when_services_are_null()
        {
            // Act
            Action act = () => new ConfigurationServiceBuilder(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
