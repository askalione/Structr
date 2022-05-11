using FluentAssertions;
using System;
using System.Linq;
using System.Text;
using Xunit;
using Structr.Abstractions.Extensions;
using System.Collections.Generic;

namespace Structr.Tests.Abstractions.Extensions
{
    public class ObjectExtensionsTests
    {
        private class Foo
        {
            public int Id { get; init; }
            public Bar BarProperty { get; init; }
            public DateTime GetDateTime()
            {
                return DateTime.Now;
            }
            public bool Flag { get; set; }
        }

        public class Bar
        {
            public int BarId { get; init; }
            public Baz BazProperty { get; init; }
        }

        public class Baz
        {
            public int BazId { get; init; }
            public string BazName { get; init; }
        }

        [Fact]
        public void Sets_property_of_simple_type()
        {
            // Arrange
            var foo = new Foo();

            // Act
            foo.SetProperty("Id", 7);

            // Assert
            foo.Id.Should().Be(7);
        }

        [Fact]
        public void Sets_property_of_custom_type()
        {
            // Arrange
            var foo = new Foo { BarProperty = new Bar() };

            // Act
            foo.SetProperty("BarProperty", new Bar { BarId = 8 });

            // Assert
            foo.BarProperty.Should().BeEquivalentTo(new Bar { BarId = 8 });
        }

        [Fact]
        public void Sets_nested_property()
        {
            // Arrange
            var foo = new Foo { BarProperty = new Bar() };

            // Act
            foo.SetProperty("BarProperty.BarId", 9);

            // Assert
            foo.BarProperty.BarId.Should().Be(9);
        }

        [Fact]
        public void Dumps_object()
        {
            // Arrange
            var foo = new Foo
            {
                Id = 1,
                BarProperty = new Bar
                {
                    BarId = 2,
                    BazProperty = new Baz
                    {
                        BazId = 3,
                        BazName = "SomeBaz"
                    }
                },
                Flag = true
            };

            // Act
            var result = foo.Dump(3);

            // Assert
            result.Should().ContainAll(new string[]
            {
                "ObjectExtensionsTests+Foo",
                "Id: 1",
                "BarProperty",
                "BarId: 2",
                "BazProperty: { }",
                "ObjectExtensionsTests+Bar",
                "Flag: True"
            });
            result.Should().NotContain("ObjectExtensionsTests+Baz");
        }
    }
}