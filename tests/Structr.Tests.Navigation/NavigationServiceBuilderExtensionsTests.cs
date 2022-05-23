using Microsoft.Extensions.DependencyInjection;
using Structr.Navigation.Internal;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationServiceBuilderExtensionsTests
    {
        [Fact]
        public void AddProvider_Json()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.json");
            var provider = new JsonNavigationProvider<InternalNavigationItem>(path);
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                    .AddProvider(provider)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<InternalNavigationItem>();
        }

        [Fact]
        public void AddProvider_Xml()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.xml");
            var provider = new XmlNavigationProvider<InternalNavigationItem>(path);
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                    .AddProvider(provider)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<InternalNavigationItem>();
        }

        [Fact]
        public void AddJson()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.json");
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                    .AddJson<InternalNavigationItem>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<InternalNavigationItem>();
        }

        [Fact]
        public void AddXml()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.xml");
            var navigationBuilder = new ServiceCollection()
                .AddNavigation();

            // Act
            var serviceProvider = navigationBuilder
                    .AddXml<InternalNavigationItem>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<InternalNavigationItem>();
        }
    }
}
