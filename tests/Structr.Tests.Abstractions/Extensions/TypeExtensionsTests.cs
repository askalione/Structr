using FluentAssertions;
using Structr.Abstractions.Extensions;
using System;
using Xunit;

namespace Structr.Tests.Abstractions.Extensions
{
    public class TypeExtensionsTests
    {
        private class Foo
        {
            public Bar BarProperty { get; set; }

#pragma warning disable 0649
            public int BarField;
#pragma warning restore 0649
        }

        public class Bar
        {
            public int BarId { get; set; }
        }

        [Theory]
        [InlineData("BarProperty", true)]
        [InlineData("BazProperty", false)]
        [InlineData("BarField", false)]
        [InlineData("BarProperty.BarId", false)]
        public void HasOwnProperty(string propertyName, bool expected)
        {
            // Act
            var result = typeof(Foo).HasOwnProperty(propertyName);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(null, "BarProperty")]
        [InlineData(typeof(Foo), "")]
        public void HasOwnProperty_throws_for_null_type_or_if_propertyName_is_empty(Type type, string propertyName)
        {
            // Act
            Action act = () => type.HasOwnProperty(propertyName);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("BarProperty.BarId", true)]
        [InlineData("BarProperty.BarName", false)]
        public void HasNestedProperty(string propertyName, bool expected)
        {
            // Act
            var result = typeof(Foo).HasNestedProperty(propertyName);

            // Assert
            result.Should().Be(expected);
        }

        private enum FooBar
        {
            Foo,
            Bar
        }

        [Theory]
        [InlineData(typeof(Foo), false)]
        [InlineData(typeof(FooBar), false)]
        [InlineData(typeof(FooBar?), true)]
        public void IsNullableEnum(Type type, bool expected)
        {
            // Act
            var result = type.IsNullableEnum();

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("BarProperty", "BarProperty")]
        [InlineData("BarProperty.BarId", "BarId")]
        [InlineData("BarProperty.BarName", null)]
        public void GetPropertyInfo(string propertyName, string expected)
        {
            // Act
            var result = typeof(Foo).GetPropertyInfo(propertyName);

            // Assert
            var foundPropertyName = result?.Name;
            foundPropertyName.Should().Be(expected);
        }

        [Theory]
        [InlineData(typeof(Foo), null)]
        [InlineData(typeof(int), 0)]
        [InlineData(typeof(int?), null)]
        [InlineData(typeof(FooBar), FooBar.Foo)]
        public void GetDefaultValue(Type type, object expected)
        {
            // Act
            var result = type.GetDefaultValue();

            // Assert
            result.Should().Be(expected);
        }

        private class FooGeneric1<T> where T : class { }
        private class FooGeneric2<T> where T : struct { }
        private class Bar1 : FooGeneric1<Bar> { }
        private class Bar2 : FooGeneric2<int> { }
        private interface IFooGeneric3<T1, T2> { }
        private class Bar3 : IFooGeneric3<int, DateTime> { }
        private class FooGeneric4<T> : IFooGeneric3<T, DateTime> { }
        private class Bar4 : FooGeneric4<Foo> { }

        [Theory]
        [InlineData(typeof(FooGeneric1<>), typeof(Bar1), true)]
        [InlineData(typeof(FooGeneric2<>), typeof(Bar1), false)]
        [InlineData(typeof(FooGeneric2<>), typeof(Bar2), true)]
        [InlineData(typeof(IFooGeneric3<,>), typeof(Bar3), true)]
        [InlineData(typeof(IFooGeneric3<,>), typeof(FooGeneric4<>), true)]
        [InlineData(typeof(IFooGeneric3<,>), typeof(Bar4), true)]
        public void IsAssignableFromGenericType(Type type1, Type type2, bool expected)
        {
            // Act
            var result = type1.IsAssignableFromGenericType(type2);

            // Assert
            result.Should().Be(expected);
        }
    }
}