using FluentAssertions;
using Structr.Navigation;
using System;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationServiceBuilderTests
    {
        [Fact]
        public void Ctor_throws_ArgumentNullException_if_services_are_null()
        {
            // Act
            Action act = () => new NavigationServiceBuilder(null); ;

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'services')");
        }
    }
}
