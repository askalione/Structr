using FluentAssertions;
using Structr.Specifications;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Structr.Tests.Specifications
{
    public class SpecificationTests
    {
        class StringLengthSpecification : Specification<string>
        {
            public int Length { get; }

            public StringLengthSpecification(int length)
            {
                Length = length;
            }

            public override Expression<Func<string, bool>> ToExpression()
            {
                return x => x.Length == Length;
            }
        }

        [Theory]
        [InlineData("abc", 3, true)]
        [InlineData("abcd", 3, false)]
        public void IsSatisfiedBy(string input, int length, bool expected)
        {
            // Arrange
            var spec = new StringLengthSpecification(length);

            // Act
            var result = spec.IsSatisfiedBy(input);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", 3, true)]
        [InlineData("abcd", 3, false)]
        public void ToFunc(string input, int length, bool expected)
        {
            // Arrange
            var spec = new StringLengthSpecification(length);

            // Act
            var result = spec.ToFunc();

            // Assert
            result(input).Should().Be(expected);
        }
    }
}
