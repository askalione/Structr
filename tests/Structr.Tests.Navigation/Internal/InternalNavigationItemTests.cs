using FluentAssertions;
using Structr.Navigation.Internal;
using System;
using Xunit;

namespace Structr.Tests.Navigation.Internal
{
    public class InternalNavigationItemTests
    {
        #region AddChild

        [Fact]
        public void Correct_relations_configuration_after_child_adding()
        {
            // Arrange

            var parent = new InternalNavigationItem();
            parent.Id = "1";
            parent.Title = "Parent";

            var child = new InternalNavigationItem();
            child.Id = "2";
            child.Title = "Child";

            // Act

            parent.AddChild(child);

            // Assert

            parent.Id.Should().Be("1");
            parent.Title.Should().Be("Parent");
            parent.ResourceName.Should().BeNull();
            parent.Children.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be("2");
                        first.Title.Should().Be("Child");
                    }
                );
            parent.Ancestors.Should().BeEmpty();
            parent.Descendants.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.SatisfyRespectively(
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
            child.Ancestors.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.SatisfyRespectively(
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
        public void ArgumentNullException_after_null_child_adding()
        {
            // Arrange

            var item = new InternalNavigationItem();
            item.Id = "1";
            item.Title = "Title";

            // Act

            Action act = () => item.AddChild(null);

            // Assert

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'child')");
        }

        #endregion

        #region RemoveChild

        [Fact]
        public void Correct_relations_configuration_after_child_removing()
        {
            // Arrange

            var root = new InternalNavigationItem();
            root.Id = "1";
            root.Title = "Root";

            var firstChild = new InternalNavigationItem();
            firstChild.Id = "2";
            firstChild.Title = "First Child";

            var secondChild = new InternalNavigationItem();
            secondChild.Id = "3";
            secondChild.Title = "Second Child";

            var thirdChild = new InternalNavigationItem();
            thirdChild.Id = "4";
            thirdChild.Title = "Third Child";

            root.AddChild(firstChild);
            root.AddChild(secondChild);
            root.AddChild(thirdChild);

            // Act

            root.RemoveChild(secondChild);

            // Assert

            root.Id.Should().Be("1");
            root.Title.Should().Be("Root");
            root.ResourceName.Should().BeNull();
            root.Children.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be("2");
                        first.Title.Should().Be("First Child");
                    },
                    second =>
                    {
                        second.Id.Should().Be("4");
                        second.Title.Should().Be("Third Child");
                    }
                );
            root.Ancestors.Should().BeEmpty();
            root.Descendants.Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.SatisfyRespectively(
                    first =>
                    {
                        first.Id.Should().Be("2");
                        first.Title.Should().Be("First Child");
                    },
                    second =>
                    {
                        second.Id.Should().Be("4");
                        second.Title.Should().Be("Third Child");
                    }
                );
            root.Parent.Should().BeNull();
            root.IsActive.Should().BeFalse();
            root.HasChildren.Should().BeTrue();
            root.HasActiveChild.Should().BeFalse();
            root.HasActiveDescendant.Should().BeFalse();
            root.HasActiveAncestor.Should().BeFalse();
        }

        [Fact]
        public void ArgumentNullException_after_null_child_removing()
        {
            // Arrange

            var item = new InternalNavigationItem();
            item.Id = "1";
            item.Title = "Title";

            // Act

            Action act = () => item.RemoveChild(null);

            // Assert

            act.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'child')");
        }

        #endregion

        #region Equals

        [Fact]
        public void Items_with_same_id_are_equal()
        {
            // Arrange

            var firstItem = new InternalNavigationItem();
            firstItem.Id = "1";
            firstItem.Title = "First Item";

            var secondItem = new InternalNavigationItem();
            secondItem.Id = "1";
            secondItem.Title = "Second Item";

            // Act

            var result = firstItem.Equals(secondItem);

            // Assert

            result.Should().BeTrue();
        }

        [Fact]
        public void Items_with_different_id_are_not_equal()
        {
            // Arrange

            var firstItem = new InternalNavigationItem();
            firstItem.Id = "1";
            firstItem.Title = "First Item";

            var secondItem = new InternalNavigationItem();
            secondItem.Id = "2";
            secondItem.Title = "Second Item";

            // Act

            var result = firstItem.Equals(secondItem);

            // Assert

            result.Should().BeFalse();
        }

        #endregion
    }
}
