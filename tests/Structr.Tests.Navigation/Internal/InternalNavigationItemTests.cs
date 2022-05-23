using FluentAssertions;
using Structr.Navigation.Internal;
using System;
using Xunit;

namespace Structr.Tests.Navigation.Internal
{
    public class InternalNavigationItemTests
    {
        [Fact]
        public void AddChild()
        {
            // Arrange
            var parent = new InternalNavigationItem { Id = "1", Title = "Parent" };
            var child = new InternalNavigationItem { Id = "2", Title = "Child" };

            // Act
            parent.AddChild(child);

            // Assert
            parent.Id.Should().Be("1");
            parent.Title.Should().Be("Parent");
            parent.ResourceName.Should().BeNull();
            parent.Children.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be("2");
                    first.Title.Should().Be("Child");
                }
            );
            parent.Ancestors.Should().BeEmpty();
            parent.Descendants.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be("2");
                    first.Title.Should().Be("Child");
                }
            );
            parent.Parent.Should().BeNull();
            parent.IsActive.Should().BeFalse();
            parent.HasChildren.Should().BeTrue();
            parent.HasActiveChild.Should().BeFalse();
            parent.HasActiveDescendant.Should().BeFalse();
            parent.HasActiveAncestor.Should().BeFalse();

            child.Id.Should().Be("2");
            child.Title.Should().Be("Child");
            child.ResourceName.Should().BeNull();
            child.Children.Should().BeEmpty();
            child.Ancestors.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be("1");
                    first.Title.Should().Be("Parent");
                }
            );
            child.Descendants.Should().BeEmpty();
            child.Parent.Should().NotBeNull();
            child.Parent.Id.Should().Be("1");
            child.Parent.Title.Should().Be("Parent");
            child.IsActive.Should().BeFalse();
            child.HasChildren.Should().BeFalse();
            child.HasActiveChild.Should().BeFalse();
            child.HasActiveDescendant.Should().BeFalse();
            child.HasActiveAncestor.Should().BeFalse();
        }

        [Fact]
        public void AddChild_throws_ArgumentNullException_when_child_is_null()
        {
            // Arrange
            var item = new InternalNavigationItem { Id = "1", Title = "Title" };

            // Act
            Action act = () => item.AddChild(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'child')");
        }

        [Fact]
        public void RemoveChild()
        {
            // Arrange
            var root = new InternalNavigationItem { Id = "1", Title = "Root" };
            var firstChild = new InternalNavigationItem { Id = "2", Title = "First Child" };
            var secondChild = new InternalNavigationItem { Id = "3", Title = "Second Child" };
            var thirdChild = new InternalNavigationItem { Id = "4", Title = "Third Child" };
            root.AddChild(firstChild);
            root.AddChild(secondChild);
            root.AddChild(thirdChild);

            // Act
            root.RemoveChild(secondChild);

            // Assert
            var expected = new InternalNavigationItem { Id = "1", Title = "Root" };
            expected.AddChild(new InternalNavigationItem { Id = "2", Title = "First Child" });
            expected.AddChild(new InternalNavigationItem { Id = "4", Title = "Third Child" });

            root.Should().BeEquivalentTo(expected, options => options.IgnoringCyclicReferences());
        }

        [Fact]
        public void RemoveChild_throws_ArgumentNullException_when_child_is_null()
        {
            // Arrange
            var item = new InternalNavigationItem { Id = "1", Title = "Title" };

            // Act
            Action act = () => item.RemoveChild(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'child')");
        }

        [Theory]
        [ClassData(typeof(EqualsTheoryData))]
        internal void EqualsMethod(InternalNavigationItem firstItem, InternalNavigationItem secondItem, bool expected)
        {
            // Act
            var result = firstItem.Equals(secondItem);

            // Assert
            result.Should().Be(expected);
        }

        private class EqualsTheoryData : TheoryData<InternalNavigationItem, InternalNavigationItem, bool>
        {
            public EqualsTheoryData()
            {
                Add(new InternalNavigationItem { Id = "1", Title = "Title 1" }, new InternalNavigationItem { Id = "1", Title = "Title 1" }, true);
                Add(new InternalNavigationItem { Id = "1", Title = "Title 1" }, new InternalNavigationItem { Id = "2", Title = "Title 1" }, false);
                Add(new InternalNavigationItem { Id = "2", Title = "Title 1" }, new InternalNavigationItem { Id = "1", Title = "Title 1" }, false);
                Add(new InternalNavigationItem { Id = "2", Title = "Title 1" }, new InternalNavigationItem { Id = "2", Title = "Title 1" }, true);

                Add(new InternalNavigationItem { Id = "1", Title = "Title 1" }, new InternalNavigationItem { Id = "1", Title = "Title 2" }, true);
                Add(new InternalNavigationItem { Id = "1", Title = "Title 1" }, new InternalNavigationItem { Id = "2", Title = "Title 2" }, false);
                Add(new InternalNavigationItem { Id = "2", Title = "Title 1" }, new InternalNavigationItem { Id = "1", Title = "Title 2" }, false);
                Add(new InternalNavigationItem { Id = "2", Title = "Title 1" }, new InternalNavigationItem { Id = "2", Title = "Title 2" }, true);
            }
        }
    }
}
