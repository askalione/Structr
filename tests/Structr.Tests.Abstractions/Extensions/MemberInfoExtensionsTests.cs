using FluentAssertions;
using Structr.Abstractions.Extensions;
using System;
using System.Linq;
using Xunit;

namespace Structr.Tests.Abstractions.Extensions
{
    public class MemberInfoExtensionsTests
    {
        private class Foo
        {
            public int BarProperty { get; set; }

            public string BarField;

            public bool BarMethod() => BarProperty == 1;

#pragma warning disable 0067
            public event EventHandler<byte> BarEvent;
#pragma warning restore 0067

            public class BarClass
            {
                public string BarProperty { get; set; }
            }
        }

        [Fact]
        public void GetMemberValue_for_property()
        {
            // Arrange
            var foo = new Foo { BarProperty = 1, BarField = "2" };

            // Act
            var result = typeof(Foo).GetMember("BarProperty").Single().GetMemberValue(foo);

            // Assert
            result.Should().Be(1);
        }

        [Fact]
        public void GetMemberValue_for_field()
        {
            // Arrange
            var foo = new Foo { BarProperty = 1, BarField = "2" };

            // Act
            var result = typeof(Foo).GetMember("BarField").Single().GetMemberValue(foo);

            // Assert
            result.Should().Be("2");
        }

        [Fact]
        public void GetMemberValue_Throws_when_getting_non_field_and_non_property_value()
        {
            // Arrange
            var foo = new Foo { BarProperty = 1, BarField = "2" };

            // Act
            Action act = () => typeof(Foo).GetMember("BarMethod").Single().GetMemberValue(foo);

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("Not supported member type");
        }

        [Fact]
        public void GetMemberType_for_property()
        {
            // Act
            var result = typeof(Foo).GetMember("BarProperty").Single().GetMemberType();

            // Assert
            result.Should().Be(typeof(int));
        }

        [Fact]
        public void GetMemberType_for_field()
        {
            // Act
            var result = typeof(Foo).GetMember("BarField").Single().GetMemberType();

            // Assert
            result.Should().Be(typeof(string));
        }

        [Fact]
        public void GetMemberType_for_method()
        {
            // Act
            var result = typeof(Foo).GetMember("BarMethod").Single().GetMemberType();

            // Assert
            result.Should().Be(typeof(bool));
        }

        [Fact]
        public void GetMemberType_for_type()
        {
            // Act
            var result = typeof(Foo).GetMember("BarEvent").Single().GetMemberType();

            // Assert
            result.Should().Be(typeof(EventHandler<byte>));
        }

        [Fact]
        public void GetMemberType_throws_when_getting_non_field_property_event_method_type()
        {
            // Act
            Action act = () => typeof(Foo).GetMember("BarClass").Single().GetMemberType();

            // Assert
            act.Should().Throw<NotSupportedException>().WithMessage("Not supported member type");
        }
    }
}