using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Configuration;
using System;
using Xunit;

namespace Structr.Tests.Configuration
{
    [Collection("Tests with temp files")]
    public class ConfigurationServiceBuilderTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = new ConfigurationServiceBuilder(services);

            // Assert
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
