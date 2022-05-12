using FluentAssertions;
using System;
using System.Linq;
using System.Text;
using Xunit;
using Structr.Abstractions.Extensions;

namespace Structr.Tests.Abstractions.Extensions
{
    public class StringExtensionsTests
    {
        private enum FooBarBaz
        {
            Foo,
            Bar,
            Baz
        }

        [Theory]
        [InlineData(null, "default")]
        [InlineData("", "default")]
        [InlineData("      ", "default")]
        [InlineData("someValue", "someValue")]
        public void Returns_default_value_when_null_or_empty(string value, string expected)
        {
            // Act
            var result = value.DefaultIfEmpty(expected);

            // Assert
            result.Should().Be(expected);
        }

        //[Theory]
        //[InlineData("qwerty", null, true)]
        //[InlineData(null, "qwerty", false)]
        //public void Contains_when_one_of_parameters_is_null(string str, string value, bool expected)
        //{
        //    // Act
        //    var result = str.Contains(value, StringComparison.CurrentCulture);

        //    // Assert
        //    result.Should().Be(expected);
        //}

        //[Theory]
        //[InlineData("wert", StringComparison.Ordinal, false)]
        //[InlineData("wert", StringComparison.OrdinalIgnoreCase, true)]
        //public void Contains(string value, StringComparison comparison, bool expected)
        //{
        //    // Arrange
        //    var str = "QwErTy";

        //    // Act
        //    var result = str.Contains(value, comparison);

        //    // Assert
        //    result.Should().Be(expected);
        //}

        [Theory]
        [InlineData("123,45", typeof(float), 123.45F)]
        [InlineData("Foo", typeof(FooBarBaz), FooBarBaz.Foo)]
        [InlineData("Bar", typeof(FooBarBaz?), FooBarBaz.Bar)]
        [InlineData("", typeof(int?), null)]
        public void Cast(string value, Type type, object expected)
        {
            // Act
            var result = value.Cast(type, true);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void Cast_with_generic_parameter()
        {
            // Act
            var result = "123,45".Cast<float>(true);

            // Assert
            result.Should().Be(123.45F);
        }

        [Fact]
        public void Throws_if_asked_when_cast_fails()
        {
            // Arrange
            var value = "123.45";
            var type = typeof(int);

            // Act
            Action act = () => value.Cast(type, true);

            // Assert
            act.Should().Throw<InvalidCastException>()
                .WithMessage($"Error with converting string \"{value}\" to type \"{type.Name}\"");
        }

        [Fact]
        public void Failed_cast_doesnt_throw_if_not_asked_and_returns_null()
        {
            // Arrange
            var value = "123.45";
            var type = typeof(int);

            // Act
            var result = value.Cast(type);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Throws_when_casting_empty_string_to_non_nullable()
        {
            // Arrange
            var value = "";
            var type = typeof(int);

            // Act
            Action act = () => value.Cast(type, true);

            // Assert
            act.Should().Throw<InvalidCastException>()
                .WithMessage($"Error with converting string \"{value}\" to type \"{type.Name}\"");
        }

        [Fact]
        public void Formats_to_hyphen_case()
        {
            // Act
            var result = "ToHyphenCase".ToHyphenCase();

            // Assert
            result.Should().Be("to-hyphen-case");
        }     

        [Fact]
        public void Formats_to_camel_case()
        {
            // Act
            var result = "ToCamelCase".ToCamelCase();

            // Assert
            result.Should().Be("toCamelCase");
        }        
    }
}