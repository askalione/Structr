using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Structr.Navigation;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using System;
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
            var navigation = new BreadcrumbNavigation<CustomNavigationItem>(builder);

            // Assert
            navigation.Active.Should().NotBeNull();
            navigation.RecursivelyCountActiveChildren().Should().Be(1);
        }

        [Fact]
        public void Ctor_throws_when_builder_is_null()
        {
            // Act
            var act = () => new BreadcrumbNavigation<CustomNavigationItem>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
