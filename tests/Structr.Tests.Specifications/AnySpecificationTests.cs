using FluentAssertions;
using Structr.Specifications;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Specifications
{
    public class AnySpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy()
        {
            // Arrange
            var strings = new[] { "abc", "bcd", "abcde", "bcde", "" };
            var spec = new AnySpecification<string>();
            List<string> result = new List<string>();

            // Act
            foreach (var item in strings)
            {
                if (spec.IsSatisfiedBy(item))
                {
                    result.Add(item);
                }
            }

            // Assert
            result.Should().Equal(strings);
        }
    }
}
