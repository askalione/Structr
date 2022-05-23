using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Structr.Navigation;
using Structr.Navigation.Internal;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationCacheTests
    {
        [Fact]
        public void GetOrAdd()
        {
            // Arrange
            var items = new List<InternalNavigationItem>
            {
                new InternalNavigationItem { Id = "1", Title = "First Item" },
                new InternalNavigationItem { Id = "2", Title = "Second Item" }
            };
            var navigationCache = new NavigationCache(new MemoryCache(new MemoryCacheOptions { SizeLimit = 1024 }));

            // Act
            var result = navigationCache.GetOrAdd(() => items);

            // Assert
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be("1");
                    first.Title.Should().Be("First Item");
                },
                second =>
                {
                    second.Id.Should().Be("2");
                    second.Title.Should().Be("Second Item");
                }
            );
        }
    }
}
