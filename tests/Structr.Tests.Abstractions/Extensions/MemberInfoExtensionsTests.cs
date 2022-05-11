using FluentAssertions;
using System;
using System.Linq;
using Xunit;
using Structr.Abstractions.Extensions;

namespace Structr.Tests.Abstractions.Extensions
{
    public class MemberInfoExtensionsTests
    {
        private class Foo
        {
            public int BarProperty { get; set; }

            public string BarField;

            public bool BarMethod() => BarProperty == 1;
        }

        [Fact]
        public void Gets_property_value()
        {
            // Arrange
            var foo = new Foo { BarProperty = 1, BarField = "2" };

            // Act
            var result = typeof(Foo).GetMember("BarProperty").Single().GetValue(foo);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void Gets_field_value()
        {
            // Arrange
            var foo = new Foo { BarProperty = 1, BarField = "2" };

            // Act
            var result = typeof(Foo).GetMember("BarField").Single().GetValue(foo);

            // Assert
            result.Should().Be("2");
        }

        [Fact]
        public void Can_get_only_field_and_property_values()
        {
            // Arrange
            var foo = new Foo { BarProperty = 1, BarField = "2" };

            // Act
            Action act = () => typeof(Foo).GetMember("BarMethod").Single().GetValue(foo);

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("Not supported member type");
        }

        [Fact]
        public void Gets_property_type()
        {
            // Act
            var result = Structr.Abstractions.Extensions.MemberInfoExtensions
                .GetType(typeof(Foo).GetMember("BarProperty").Single());

            // Assert
            result.Should().Be(typeof(int));
        }

        [Fact]
        public void Gets_field_type()
        {
            // Act
            var result = Structr.Abstractions.Extensions.MemberInfoExtensions
                .GetType(typeof(Foo).GetMember("BarField").Single());

            // Assert
            result.Should().Be(typeof(string));
        }

        [Fact]
        public void Can_get_only_field_and_property_types()
        {
            // Act
            Action act = () => Structr.Abstractions.Extensions.MemberInfoExtensions
                .GetType(typeof(Foo).GetMember("BarMethod").Single());

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("Not supported member type");
        }
    }
}