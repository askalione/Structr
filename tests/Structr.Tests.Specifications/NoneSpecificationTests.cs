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

        private class EqualsTestTheoryData : TheoryData<NoneSpecification<string>, object, bool>
        {
            public EqualsTestTheoryData()
            {
                Add(new NoneSpecification<string>(), null, false);
                Add(new NoneSpecification<string>(), new NoneSpecification<string>(), true);
                Add(new NoneSpecification<string>(), new AnySpecification<string>(), false);
                var spec = new NoneSpecification<string>();
                Add(spec, spec, true);
            }
        }

        [Theory]
        [ClassData(typeof(EqualsTestTheoryData))]
        public void EqualsTest(NoneSpecification<string> spec1, object spec2, bool expected)
        {
            // Act
            var result = spec1.Equals(spec2);

            // Arrange
            result.Should().Be(expected);
        }
    }
}
