using FluentAssertions;
using Structr.Abstractions;
using System;
using Xunit;

namespace Structr.Tests.Abstractions
{
    public class HierarchyIdTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new HierarchyId(new int[] { 1, 2, 3 });

            // Assert
            result.ToString().Should().Be("/1/2/3/");
        }

        [Fact]
        public void Ctor_throws_without_ids_collection()
        {
            // Act
            Action act = () => new HierarchyId(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetNode()
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 1, 2, 3 });

            // Act
            var result = hierarchyId.GetNode();

            // Assert
            result.Should().Be(3);
        }

        [Theory]
        [InlineData(3, true)]
        [InlineData(5, false)]
        [InlineData(7, false)]
        public void IsDescendantOf(int id, bool expected)
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 1, 2, 3, 4, 5 });

            // Act
            var result = hierarchyId.IsDescendantOf(id);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(1, "/1/2/3/4/")]
        [InlineData(3, "/1/2/")]
        [InlineData(10, "//")]
        public void GetAncestor(int levelsHigher, string expected)
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 1, 2, 3, 4, 5 });

            // Act
            var result = hierarchyId.GetAncestor(levelsHigher);

            // Assert
            result.ToString().Should().Be(expected);
        }

        [Fact]
        public void GetAncestor_throws_when_levels_count_less_then_1()
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 1, 2, 3, 4, 5 });

            // Act
            Action act = () => hierarchyId.GetAncestor(0);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void GetDescendant()
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 1, 2, 3, 4, 5 });

            // Act
            var result = hierarchyId.GetDescendant(6);

            // Assert
            result.ToString().Should().Be("/1/2/3/4/5/6/");
        }

        [Fact]
        public void GetLevel()
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 1, 2, 3, 4, 5 });

            // Act
            var result = hierarchyId.GetLevel();

            // Assert
            result.Should().Be(5);
        }

        [Fact]
        public void Move()
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 11, 12, 13, 14, 15 });
            var ancestor1 = new HierarchyId(new int[] { 11, 12, 13 });
            var ancestor2 = new HierarchyId(new int[] { 21, 22 });

            // Act
            hierarchyId.Move(ancestor1, ancestor2);

            // Assert
            hierarchyId.ToString().Should().Be("/21/22/14/15/");
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void Move_throws_when_one_of_argunments_is_null(bool firstIsNull, bool secondIsNull)
        {
            // Arrange
            var hierarchyId = new HierarchyId(new int[] { 11, 12, 13, 14, 15 });
            HierarchyId ancestor1 = firstIsNull ? null : new HierarchyId(new int[] { 11, 12, 13 });
            HierarchyId ancestor2 = secondIsNull ? null : new HierarchyId(new int[] { 21, 22 });

            // Act
            Action act = () => hierarchyId.Move(ancestor1, ancestor2);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
