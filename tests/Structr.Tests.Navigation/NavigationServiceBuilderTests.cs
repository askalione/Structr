using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Structr.Navigation;
using System;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationServiceBuilderTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = new NavigationServiceBuilder(services);

            // Assert
            result.Should().NotBeNull();
            result.Services.Should().BeEquivalentTo(services);
        }

        [Fact]
        public void Ctor_throws_if_services_are_null()
        {
            // Act
            Action act = () => new NavigationServiceBuilder(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}