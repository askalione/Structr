using Microsoft.Extensions.DependencyInjection;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationServiceBuilderExtensionsTests
    {
        [Fact]
        public void AddProvider_json()
        {
            // Arrange
            var path = TestDataPath.Combine("menu.json");
            var provider = new JsonNavigationProvider<CustomNavigationItem>(path);
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                .AddProvider(provider)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<CustomNavigationItem>();
        }

        [Fact]
        public void AddProvider_xml()
        {
            // Arrange
            var path = TestDataPath.Combine("menu.xml");
            var provider = new XmlNavigationProvider<CustomNavigationItem>(path);
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                .AddProvider(provider)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<CustomNavigationItem>();
        }
    }
}
