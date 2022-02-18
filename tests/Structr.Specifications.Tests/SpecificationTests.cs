using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Structr.Specifications.Tests
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
        [InlineData("abc", 3, true)]
        [InlineData("abcd", 3, false)]
        public void IsSatisfiedBy_StringCandidateAndLengthCriteria_ExpectedValue(string input, int length, bool expected)
        {
            var spec = new StringLengthSpecification(length);

            Assert.Equal(expected, spec.IsSatisfiedBy(input));
        }

        [Fact]
        public void And_SameLinqPredicate_SameResults()
        {
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringContainsSpecification("de");

            var spec = spec1.And(spec2);
            var actual = StringCollection.Items.Where(x => spec.IsSatisfiedBy(x));
            var expected = StringCollection.Items.Where(x => x.Contains(spec1.Part) && x.Contains(spec2.Part));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Or_SameLinqPredicate_SameResults()
        {
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringContainsSpecification("de");

            var spec = spec1.Or(spec2);
            var actual = StringCollection.Items.Where(x => spec.IsSatisfiedBy(x));
            var expected = StringCollection.Items.Where(x => x.Contains(spec1.Part) || x.Contains(spec2.Part));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Not_SameLinqPredicate_SameResults()
        {
            var spec1 = new StringContainsSpecification("ab");

            var spec = spec1.Not();
            var actual = StringCollection.Items.Where(x => spec.IsSatisfiedBy(x));
            var expected = StringCollection.Items.Where(x => !x.Contains(spec1.Part));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AndNot_SameLinqPredicate_SameResults()
        {
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringLengthSpecification(3);

            var spec = spec1.AndNot(spec2);
            var actual = StringCollection.Items.Where(x => spec.IsSatisfiedBy(x));
            var expected = StringCollection.Items.Where(x => x.Contains(spec1.Part) && x.Length != spec2.Length);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void OrNot_SameLinqPredicate_SameResults()
        {
            var spec1 = new StringContainsSpecification("ab");
            var spec2 = new StringLengthSpecification(3);

            var spec = spec1.Or(spec2);
            var actual = StringCollection.Items.Where(x => spec.IsSatisfiedBy(x));
            var expected = StringCollection.Items.Where(x => x.Contains(spec1.Part) || x.Length != spec2.Length);

            Assert.Equal(expected, actual);
        }
    }
}
