using Microsoft.Extensions.DependencyInjection;
using Structr.Navigation.Internal;
using Structr.Tests.Navigation.TestUtils;
using Structr.Tests.Navigation.TestUtils.Extensions;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddNavigation()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.json");

            // Act
            var serviceProvider = new ServiceCollection()
                .AddNavigation()
                    .AddJson<InternalNavigationItem>(path)
                .Services
                .BuildServiceProvider();

            // Assert
            serviceProvider.ShouldContainsNavigationServices<InternalNavigationItem>();
        }
    }
}
