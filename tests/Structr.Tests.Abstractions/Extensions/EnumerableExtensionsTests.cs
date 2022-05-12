using System.Collections.Generic;
using System.Linq;
using Xunit;
using Structr.Abstractions.Extensions;
using Structr.Abstractions;
using FluentAssertions;

namespace Structr.Tests.Abstractions.Extensions
{
    public class EnumerableExtensionsTests
    {
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
            };

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
            };
            result.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }
    }
}
