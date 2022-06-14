using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Structr.Navigation;

namespace Structr.Tests.Navigation.TestUtils.Extensions
{
    internal static class ServiceProviderExtensions
    {
        public static void ShouldContainsNavigationServices<TNavigationItem>(this ServiceProvider serviceProvider)
            where TNavigationItem : NavigationItem<TNavigationItem>, new()
        {
            serviceProvider.GetService<IMemoryCache>().Should().NotBeNull();
            serviceProvider.GetService<INavigationCache>().Should().NotBeNull();
            serviceProvider.GetService<INavigationBuilder<TNavigationItem>>().Should().NotBeNull();
            serviceProvider.GetService<INavigation<TNavigationItem>>().Should().NotBeNull();
            serviceProvider.GetService<IBreadcrumbNavigation<TNavigationItem>>().Should().NotBeNull();
        }
    }
}
