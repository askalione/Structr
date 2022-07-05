using FluentAssertions;
using Structr.Specifications;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.Specifications
{
    public class NoneSpecificationTests
    {
        [Fact]
        public void IsSatisfiedBy()
        {
            // Arrange
            var strings = new[] { "abc", "bcd", "abcde", "bcde", "" };
            var spec = new NoneSpecification<string>();
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
            result.Should().BeEmpty();
        }

        private class EqualsTestTheoryData : TheoryData<object, bool>
        {
            public EqualsTestTheoryData()
            {
                Add(null, false);                
                Add(new NoneSpecification<string>(), true);
                Add(new AnySpecification<string>(), false);
            }
        }

        [Theory]
        [ClassData(typeof(EqualsTestTheoryData))]
        public void EqualsTest(object spec2, bool expected)
        {
            // Arrange
            var spec1 = new NoneSpecification<string>();

            // Act
            var result = spec1.Equals(spec2);

            // Arrange
            result.Should().Be(expected);
        }
    }
}
