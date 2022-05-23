using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Structr.Navigation;
using Structr.Navigation.Internal;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class BreadcrumbNavigationTests
    {
        [Fact]
        public void Ctor_single_active_item()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.json");
            var provider = new JsonNavigationProvider<InternalNavigationItem>(path);
            var options = new NavigationOptions<InternalNavigationItem>
            {
                ItemActivator = item =>
                {
                    return true;
                }
            };
            var navigationCache = new NavigationCache(new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 }));
            var builder = new NavigationBuilder<InternalNavigationItem>(provider, options, navigationCache);

            // Act
            var navigation = new BreadcrumbNavigation<InternalNavigationItem>(builder);

            // Assert
            navigation.Active.Should().NotBeNull();
            navigation.RecursivelyCountActiveChildren().Should().Be(1);
        }
    }
}
