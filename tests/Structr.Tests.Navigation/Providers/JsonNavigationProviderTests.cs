using FluentAssertions;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using System;
using System.IO;
using Xunit;

namespace Structr.Tests.Navigation.Providers
{
    public class JsonNavigationProviderTests
    {
        [Fact]
        public void Ctor_throws_when_path_is_null()
        {
            // Act
            Action act = () => new JsonNavigationProvider<CustomNavigationItem>(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void CreateNavigation()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.json");
            var provider = new JsonNavigationProvider<CustomNavigationItem>(path);

            // Act
            var result = provider.CreateNavigation();

            // Assert
            var expected = MenuBuilder.Build();
            result.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
        }

        [Fact]
        public void CreateNavigation_throws_if_file_not_exist()
        {
            // Arrange
            var provider = new JsonNavigationProvider<CustomNavigationItem>("menu.json");

            // Act
            Action act = () => provider.CreateNavigation();

            // Assert
            act.Should().ThrowExactly<FileNotFoundException>();
        }
    }
}
