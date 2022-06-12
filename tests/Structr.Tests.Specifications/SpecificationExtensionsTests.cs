using FluentAssertions;
using Structr.Specifications;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Structr.Tests.Specifications
{
    public class SpecificationExtensionsTests
    {
        class StringContainsSpecification : Specification<string>
        {
            public string Part { get; }

            public StringContainsSpecification(string part)
            {
                Part = part;
            }

            public override Expression<Func<string, bool>> ToExpression()
            {
                return x => x.Contains(Part);
            }
        }

        [Theory]
        [InlineData("abc", false)]
        [InlineData("bcd", false)]
        [InlineData("abcde", true)]
        [InlineData("bcde", false)]
        public void And(string val, bool expected)
        {
            // Arrange
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringContainsSpecification("de");

            // Act
            var result = spec1.And(spec2);

            // Assert
            result.IsSatisfiedBy(val).Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", true)]
        [InlineData("bcd", false)]
        [InlineData("abcde", true)]
        [InlineData("bcde", true)]
        public void Or(string val, bool expected)
        {
            // Arrange
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringContainsSpecification("de");

            // Act
            var result = spec1.Or(spec2);

            // Assert
            result.IsSatisfiedBy(val).Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", false)]
        [InlineData("bcd", true)]
        [InlineData("abcde", false)]
        [InlineData("bcde", true)]
        public void Not(string val, bool expected)
        {
            // Arrange
            var spec1 = new StringContainsSpecification("ab");

            // Act
            var result = spec1.Not();

            // Assert
            result.IsSatisfiedBy(val).Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", true)]
        [InlineData("bcd", false)]
        [InlineData("abcde", false)]
        [InlineData("bcde", false)]
        public void AndNot(string val, bool expected)
        {
            // Arrange
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringContainsSpecification("de");

            // Act
            var result = spec1.AndNot(spec2);

            // Assert
            result.IsSatisfiedBy(val).Should().Be(expected);
        }

        [Theory]
        [InlineData("abc", true)]
        [InlineData("bcd", true)]
        [InlineData("abcde", true)]
        [InlineData("bcde", false)]
        public void OrNot(string val, bool expected)
        {
            // Arrange
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringContainsSpecification("de");

            // Act
            var result = spec1.OrNot(spec2);

            // Assert
            result.IsSatisfiedBy(val).Should().Be(expected);
        }
    }
}
