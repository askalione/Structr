using Xunit;
using FluentAssertions;
using Structr.Operations;
using System.Threading.Tasks;
using System;

namespace Structr.Tests.Operations
{
    public class VoidResultTests
    {
        [Fact]
        public async Task Uniqueness()
        {
            // Arrange
            VoidResult voidResult = new VoidResult();

            // Act Assert
            voidResult.Should().Be(VoidResult.Value);
            (await Task.FromResult(voidResult)).Should().Be(await VoidResult.TaskValue);
            default(VoidResult).Should().Be(VoidResult.Value);
        }

        [Fact]
        public void Equality()
        {
            // Arrange
            VoidResult voidResult = new VoidResult();
            VoidResult otherVoidResult = new VoidResult();

            // Act Assert
            voidResult.Equals(otherVoidResult).Should().BeTrue();
            voidResult.Equals("someNotVoidResultObject").Should().BeFalse();
            voidResult.Equals(otherVoidResult as object).Should().BeTrue();
            (voidResult == otherVoidResult).Should().BeTrue();
            (voidResult != otherVoidResult).Should().BeFalse();
        }

        [Fact]
        public void Comparing()
        {
            // Arrange
            VoidResult voidResult = new VoidResult();
            VoidResult otherVoidResult = new VoidResult();

            // Act Assert
            voidResult.CompareTo(otherVoidResult).Should().Be(0);
            voidResult.CompareTo(otherVoidResult as object).Should().Be(0);
        }

        [Fact]
        public void Comparing_throws_for_another_type()
        {
            // Arrange
            VoidResult voidResult = new VoidResult();

            // Act
            Action act = () => voidResult.CompareTo("someNotVoidResultObject");

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void HashCode()
        {
            // Arrange
            VoidResult voidResult = new VoidResult();

            // Act
            var result = voidResult.GetHashCode();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void ToStringTest()
        {
            // Arrange
            VoidResult voidResult = new VoidResult();

            // Act
            var result = voidResult.ToString();

            // Assert
            result.Should().BeEmpty();
        }
    }
}