using FluentAssertions;
using Structr.Navigation.Internal;
using Structr.Navigation.Providers;
using Structr.Tests.Navigation.TestUtils;
using System;
using System.IO;
using Xunit;

namespace Structr.Tests.Navigation.Providers
{
    public class XmlNavigationProviderTests
    {
        [Fact]
        public void Ctor_throws_ArgumentNullException_if_path_is_null()
        {
            // Act
            Action act = () => new XmlNavigationProvider<InternalNavigationItem>(null); ;

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'path')");
        }

        [Fact]
        public void CreateNavigation()
        {
            // Arrange
            var path = TestDataDirectoryPath.Combine("menu.xml");
            var provider = new XmlNavigationProvider<InternalNavigationItem>(path);

            // Act
            var result = provider.CreateNavigation();

            // Assert
            var expected = MenuBuilder.Build();
            result.Should().BeEquivalentTo(expected, opt => opt.IgnoringCyclicReferences());
        }

        [Fact]
        public void CreateNavigation_throws_FileNotFoundException_if_file_not_exist()
        {
            // Arrange
            var provider = new XmlNavigationProvider<InternalNavigationItem>("menu.xml");

            // Act
            Action act = () => provider.CreateNavigation();

            // Assert
            act.Should().ThrowExactly<FileNotFoundException>();
        }
    }
}
