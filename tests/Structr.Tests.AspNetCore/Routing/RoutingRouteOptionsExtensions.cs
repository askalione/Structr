using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Structr.AspNetCore.Routing;
using Xunit;

namespace Structr.Tests.AspNetCore.Routing
{
    public class RoutingRouteOptionsExtensions
    {
        [Fact]
        public void AddSlugifyRouting()
        {
            // Arrange
            var routeOptions = new RouteOptions();

            // Act
            routeOptions.AddSlugifyRouting("slugify_test");

            // Assert
            routeOptions.ConstraintMap.Should().Contain(x => x.Key == "slugify_test" && x.Value == typeof(SlugifyParameterTransformer));
        }
    }
}
