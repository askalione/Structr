using Microsoft.Extensions.DependencyInjection;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddNavigation_json()
        {
            // Arrange
            var path = TestDataPath.Combine("menu.json");
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                .AddJson<CustomNavigationItem>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<CustomNavigationItem>();
        }

        [Fact]
        public void AddNavigation_xml()
        {
            // Arrange
            var path = TestDataPath.Combine("menu.xml");
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                .AddXml<CustomNavigationItem>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<CustomNavigationItem>();
        }
    }
}
