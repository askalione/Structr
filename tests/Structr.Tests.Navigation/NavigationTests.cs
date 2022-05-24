using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Structr.Navigation;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationTests
    {
        [Fact]
        public void Ctor_single_active_item()
        {
            // Arrange
            var path = TestDataPath.Combine("menu.json");
            var provider = new JsonNavigationProvider<CustomNavigationItem>(path);
            var options = new NavigationOptions<CustomNavigationItem>
            {
                ItemActivator = item =>
                {
                    return true;
                }
            };
            var navigationCache = new NavigationCache(new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 }));
            var builder = new NavigationBuilder<CustomNavigationItem>(provider, options, navigationCache);

            // Act
            var navigation = new Navigation<CustomNavigationItem>(builder);

            // Assert
            navigation.Active.Should().NotBeNull();
            navigation.RecursivelyCountActiveChildren().Should().Be(1);
        }
    }
}
