using FluentAssertions;
using Structr.Abstractions.Extensions;
using System;
using System.Linq;
using Xunit;

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
        public void DefaultIfEmpty(string value, string expected)
        {
            // Act
            var result = value.DefaultIfEmpty(expected);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("QwErTy", "wert", StringComparison.Ordinal, false)]
        [InlineData("QwErTy", "wert", StringComparison.OrdinalIgnoreCase, true)]
        public void Contains(string str, string value, StringComparison comparison, bool expected)
        {
            // Act
            var result = StringExtensions.Contains(str, value, comparison);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("qwerty", null, true)]
        [InlineData(null, "qwerty", false)]
        public void Contains_when_one_of_parameters_is_null(string str, string value, bool expected)
        {
            // Act
            var result = StringExtensions.Contains(str, value, StringComparison.CurrentCulture);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("QwErTyQwErTy", "WeR", "123", StringComparison.Ordinal, "QwErTyQwErTy")]
        [InlineData("QwErTyQwErTy", "WeR", "123", StringComparison.OrdinalIgnoreCase, "Q123TyQ123Ty")]
        [InlineData("", "WeR", "123", StringComparison.OrdinalIgnoreCase, "")]
        public void Replace(string @string, string oldValue, string newValue, StringComparison comparisonType, string expected)
        {
            // Act
            var result = @string.Replace(oldValue, newValue, comparisonType);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("qwerty", "")]
        [InlineData(null, "wer")]
        public void Replace_throws_when_source_is_null_or_oldValue_is_null_or_empty(string @string, string oldValue)
        {
            // Act
            Action act = () => StringExtensions.Replace(@string, oldValue, "123", StringComparison.CurrentCulture);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("123", typeof(int), 123)]
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
            var result = "123".Cast<int>(true);

            // Assert
            result.Should().Be(123);
        }

        [Fact]
        public void Cast_throws_if_asked_when_cast_fails()
        {
            // Arrange
            var value = "123";
            var type = typeof(bool);

            // Act
            Action act = () => value.Cast(type, true);

            // Assert
            act.Should().Throw<InvalidCastException>()
                .WithMessage($"Error with converting string \"{value}\" to type \"{type.Name}\"");
        }

        [Fact]
        public void Cast_doesnt_throw_if_not_asked_and_returns_null()
        {
            // Arrange
            var value = "123";
            var type = typeof(bool);

            // Act
            var result = value.Cast(type);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Cast_throws_if_empty_string_to_non_nullable()
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
        public void ToHyphenCase()
        {
            // Act
            var result = "ToHyphenCase".ToHyphenCase();

            // Assert
            result.Should().Be("to-hyphen-case");
        }

        [Fact]
        public void ToCamelCase()
        {
            // Act
            var result = "ToCamelCase".ToCamelCase();

            // Assert
            result.Should().Be("toCamelCase");
        }

        [Fact]
        public void ToSnakeCase()
        {
            // Act
            var result = "ToSnakeCase".ToSnakeCase();

            // Assert
            result.Should().Be("to_snake_case");
        }
    }
}