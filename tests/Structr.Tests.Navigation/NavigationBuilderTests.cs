using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Structr.Navigation;
using Structr.Navigation.Internal;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationBuilderTests
    {
        [Fact]
        public void BuildNavigation()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.json");
            var provider = new JsonNavigationProvider<InternalNavigationItem>(path);
            var options = new NavigationOptions<InternalNavigationItem>();
            var navigationCache = new NavigationCache(new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 }));
            var builder = new NavigationBuilder<InternalNavigationItem>(provider, options, navigationCache);

            // Act
            var result = builder.BuildNavigation();

            // Assert
            var expected = MenuBuilder.Build();
            result.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
        }
    }
}
