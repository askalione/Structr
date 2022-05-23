using FluentAssertions;
using Structr.Abstractions;
using Structr.Abstractions.Extensions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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

        [Fact]
        public void ForEach()
        {
            // Arrange
            var list = new List<FooBar>
            {
                new FooBar { Foo = 1 },
                new FooBar { Foo = 2 },
                new FooBar { Foo = 3 },
                new FooBar { Foo = 4 }
            };

            // Act
            list.ForEach(x => x.Foo = x.Foo + 5);

            // Assert
            var expected = new List<FooBar>
            {
                new FooBar { Foo = 6 },
                new FooBar { Foo = 7 },
                new FooBar { Foo = 8 },
                new FooBar { Foo = 9 }
            };
            list.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void ForEachOrBreak()
        {
            // Arrange
            var list = new List<FooBar>
            {
                new FooBar { Foo = 1 },
                new FooBar { Foo = 2 },
                new FooBar { Foo = 3 },
                new FooBar { Foo = 4 }
            };

            // Act
            list.ForEachOrBreak(x =>
            {
                if (x.Foo >= 3)
                {
                    return true;
                }
                x.Foo = x.Foo + 5;
                return false;
            });

            // Assert
            var expected = new List<FooBar>
            {
                new FooBar { Foo = 6 },
                new FooBar { Foo = 7 },
                new FooBar { Foo = 3 },
                new FooBar { Foo = 4 }
            };
            list.Should().BeEquivalentTo(expected, opt => opt.WithStrictOrdering());
        }
    }
}
