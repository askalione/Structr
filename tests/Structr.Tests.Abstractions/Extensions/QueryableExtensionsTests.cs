using System.Collections.Generic;
using System.Linq;
using Xunit;
using Structr.Abstractions.Extensions;
using Structr.Abstractions;
using FluentAssertions;
using System;

namespace Structr.Tests.Abstractions.Extensions
{
    public class QueryableExtensionsTests
    {
        [Fact]
        public void PageBy()
        {
            // Arrange
            var queryable = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();

            // Act
            var result = queryable.PageBy(2, 5);

            // Assert
            result.Should().BeEquivalentTo(new int[] { 2, 3, 4, 5, 6 }.AsQueryable(), opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void PageBy_throws_when_query_is_null()
        {
            // Arrange
            IQueryable<int> queryable = null;

            // Act
            Action act = () => queryable.PageBy(2, 5);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(-1, 1)]
        public void PageBy_throws_whith_skip_lt_0_or_take_lt_1(int skip, int take)
        {
            // Arrange
            var queryable = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable();

            // Act
            Action act = () => queryable.PageBy(skip, take);

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        private class FooBar
        {
            public int Foo { get; set; }
            public int Bar { get; set; }
            public int Baz { get; set; }
        }

        [Fact]
        public void OrderBy()
        {
            // Arrange
            var list = new List<FooBar>
            {
                new FooBar { Foo = 5, Bar = 1, Baz = 7 },
                new FooBar { Foo = 2, Bar = 3, Baz = 2 },
                new FooBar { Foo = 2, Bar = 2, Baz = 3 },
                new FooBar { Foo = 6, Bar = 3, Baz = 2 },
                new FooBar { Foo = 6, Bar = 3, Baz = 1 },
                new FooBar { Foo = 2, Bar = 4, Baz = 4 }
            }.AsQueryable();

            // Act
            var result = list.OrderBy(new Dictionary<string, Order>
            {
                { "Foo", Order.Asc },
                { "Bar", Order.Desc },
                { "Baz", Order.Asc }
            });

            // Assert
            var expected = new List<FooBar>
            {
                new FooBar { Foo = 2, Bar = 4, Baz = 4 },
                new FooBar { Foo = 2, Bar = 3, Baz = 2 },
                new FooBar { Foo = 2, Bar = 2, Baz = 3 },
                new FooBar { Foo = 5, Bar = 1, Baz = 7 },
                new FooBar { Foo = 6, Bar = 3, Baz = 1 },
                new FooBar { Foo = 6, Bar = 3, Baz = 2 }
            }.AsQueryable();
            result.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }
    }
}
