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


    }
}