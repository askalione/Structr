using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Structr.Navigation;
using Structr.Navigation.Internal;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils.Extensions;
using System.IO;
using System.Reflection;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationTests
    {
        [Fact]
        public void Single_active_item_after_build()
        {
            // Arrange

            var path = Path.Combine(
                new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.Parent.Parent.FullName,
                "Data/menu.json");
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

            var navigation = new Navigation<InternalNavigationItem>(builder);

            // Assert

            navigation.Active.Should().NotBeNull();
            navigation.RecursivelyCountActiveChildren().Should().Be(1);
        }
    }
}
