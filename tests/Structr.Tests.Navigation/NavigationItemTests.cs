using FluentAssertions;
using Structr.Tests.Navigation.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.Navigation
{
    public class NavigationItemTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var item = new CustomNavigationItem { Id = "1", Title = "Parent" };

            // Assert
            item.Id.Should().Be("1");
            item.Title.Should().Be("Parent");
            item.ResourceName.Should().BeNull();
            item.Children.Should().BeEmpty();
            item.Ancestors.Should().BeEmpty();
            item.Descendants.Should().BeEmpty();
            item.Parent.Should().BeNull();
            item.IsActive.Should().BeFalse();
            item.HasChildren.Should().BeFalse();
            item.HasActiveChild.Should().BeFalse();
            item.HasActiveDescendant.Should().BeFalse();
            item.HasActiveAncestor.Should().BeFalse();
        }

        [Fact]
        public void AddChild()
        {
            // Arrange
            var parent = new CustomNavigationItem { Id = "1", Title = "Parent" };
            var child = new CustomNavigationItem { Id = "2", Title = "Child" };

            // Act
            parent.AddChild(child);

            // Assert
            parent.Children.Should().ContainSingle(x => x == child);
            parent.Ancestors.Should().BeEmpty();
            parent.Descendants.Should().ContainSingle(x => x == child);

            child.Children.Should().BeEmpty();
            child.Ancestors.Should().ContainSingle(x => x == parent);
            child.Descendants.Should().BeEmpty();
        }

        [Fact]
        public void AddChild_throws_when_child_is_null()
        {
            // Arrange
            var item = new CustomNavigationItem { Id = "1", Title = "Title" };

            // Act
            Action act = () => item.AddChild(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void RemoveChild()
        {
            // Arrange
            var root = new CustomNavigationItem { Id = "1", Title = "Root" };
            var firstChild = new CustomNavigationItem { Id = "2", Title = "First Child" };
            var secondChild = new CustomNavigationItem { Id = "3", Title = "Second Child" };
            root.AddChild(firstChild);
            root.AddChild(secondChild);

            // Act
            root.RemoveChild(secondChild);

            // Assert
            root.Children.Should().NotContain(x => x == secondChild);
        }

        [Fact]
        public void RemoveChild_throws_when_child_is_null()
        {
            // Arrange
            var item = new CustomNavigationItem { Id = "1", Title = "Title" };

            // Act
            Action act = () => item.RemoveChild(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        internal void EqualsTest(CustomNavigationItem firstItem, CustomNavigationItem secondItem, bool expected)
        {
            // Act
            var result = firstItem.Equals(secondItem);

            // Assert
            result.Should().Be(expected);
        }

        private class EqualsTheoryData : TheoryData<CustomNavigationItem, CustomNavigationItem, bool>
        {
            public EqualsTheoryData()
            {
                Add(new CustomNavigationItem { Id = "1", Title = "Title 1" }, new CustomNavigationItem { Id = "1", Title = "Title 1" }, true);
                Add(new CustomNavigationItem { Id = "1", Title = "Title 1" }, new CustomNavigationItem { Id = "2", Title = "Title 1" }, false);
                Add(new CustomNavigationItem { Id = "2", Title = "Title 1" }, new CustomNavigationItem { Id = "1", Title = "Title 1" }, false);
                Add(new CustomNavigationItem { Id = "2", Title = "Title 1" }, new CustomNavigationItem { Id = "2", Title = "Title 1" }, true);

                Add(new CustomNavigationItem { Id = "1", Title = "Title 1" }, new CustomNavigationItem { Id = "1", Title = "Title 2" }, true);
                Add(new CustomNavigationItem { Id = "1", Title = "Title 1" }, new CustomNavigationItem { Id = "2", Title = "Title 2" }, false);
                Add(new CustomNavigationItem { Id = "2", Title = "Title 1" }, new CustomNavigationItem { Id = "1", Title = "Title 2" }, false);
                Add(new CustomNavigationItem { Id = "2", Title = "Title 1" }, new CustomNavigationItem { Id = "2", Title = "Title 2" }, true);
            }
        }
    }
}
