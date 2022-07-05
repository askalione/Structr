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

        private class EqualsTestTheoryData : TheoryData<AnySpecification<string>, object, bool>
        {
            public EqualsTestTheoryData()
            {
                Add(new AnySpecification<string>(), null, false);
                Add(new AnySpecification<string>(), new AnySpecification<string>(), true);
                Add(new AnySpecification<string>(), new NoneSpecification<string>(), false);
                var spec = new AnySpecification<string>();
                Add(spec, spec, true);
            }
        }

        [Theory]
        [ClassData(typeof(EqualsTestTheoryData))]
        public void EqualsTest(AnySpecification<string> spec1, object spec2, bool expected)
        {
            // Act
            var result = spec1.Equals(spec2);

            // Arrange
            result.Should().Be(expected);
        }
    }
}
