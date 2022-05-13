using FluentAssertions;
using Structr.Navigation.Internal;
using Structr.Navigation.Providers;
using System;
using System.IO;
using System.Reflection;
using Xunit;

namespace Structr.Tests.Navigation.Providers
{
    public class XmlNavigationProviderTests
    {
        [Fact]
        public void Correct_navigation_after_load_from_json_file()
        {
            // Arrange

            var path = Path.Combine(
                new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Parent.Parent.Parent.FullName,
                "Data/menu.xml");
            var provider = new XmlNavigationProvider<InternalNavigationItem>(path);

            // Act

            var result = provider.CreateNavigation();

            // Assert

            result.Should().NotBeEmpty()
                .And.HaveCount(3)
                .And.SatisfyRespectively(
                    firstParent =>
                    {
                        firstParent.Id.Should().Be("Parent_1");
                        firstParent.Title.Should().Be("Parent 1");
                        firstParent.Children.Should().NotBeEmpty()
                            .And.HaveCount(2)
                            .And.SatisfyRespectively(
                                firstChild =>
                                {
                                    firstChild.Id.Should().Be("Child_1_1");
                                    firstChild.Title.Should().Be("Child 1 1");
                                    firstChild.Children.Should().NotBeEmpty()
                                        .And.HaveCount(1)
                                        .And.SatisfyRespectively(
                                            grandson =>
                                            {
                                                grandson.Id.Should().Be("Child_1_1_1");
                                                grandson.Title.Should().Be("Child 1 1 1");
                                                grandson.Children.Should().BeEmpty();
                                            }
                                        );
                                },
                                secondChild =>
                                {
                                    secondChild.Id.Should().Be("Child_1_2");
                                    secondChild.Title.Should().Be("Child 1 2");
                                    secondChild.Children.Should().BeEmpty();
                                }
                            );
                    },
                    secondParent =>
                    {
                        secondParent.Id.Should().Be("Parent_2");
                        secondParent.Title.Should().Be("Parent 2");
                        secondParent.Children.Should().NotBeEmpty()
                            .And.HaveCount(1)
                            .And.SatisfyRespectively(
                                firstChild =>
                                {
                                    firstChild.Id.Should().Be("Child_2_1");
                                    firstChild.Title.Should().Be("Child 2 1");
                                    firstChild.Children.Should().BeEmpty();
                                }
                            );
                    },
                    thirdParent =>
                    {
                        thirdParent.Id.Should().Be("Parent_3");
                        thirdParent.Title.Should().Be("Parent 3");
                        thirdParent.Children.Should().BeEmpty();
                    }
                );
        }

        [Fact]
        public void ArgumentNullException_if_file_path_is_null()
        {
            // Arrange

            // Act
            Action act = () => new XmlNavigationProvider<InternalNavigationItem>(null); ;

            // Assert
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'path')");
        }

        [Fact]
        public void FileNotFoundException_if_file_not_exist()
        {
            // Arrange
            var provider = new XmlNavigationProvider<InternalNavigationItem>("menu.xml");

            // Act
            Action act = () => provider.CreateNavigation();

            // Assert
            act.Should().Throw<FileNotFoundException>();
        }
    }
}
