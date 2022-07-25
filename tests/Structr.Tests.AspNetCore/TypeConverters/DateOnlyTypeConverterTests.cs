using FluentAssertions;
using Structr.AspNetCore.TypeConverters;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.TypeConverters
{
    public class DateOnlyTypeConverterTests
    {
        [Fact]
        public void ConvertFrom()
        {
            // Arrange
            var converter = new DateOnlyTypeConverter();

            // Act
            var result = converter.ConvertFrom("1993-05-11");

            // Assert
            result.Should().BeOfType<DateOnly>();
            result.Should().BeEquivalentTo(new DateOnly(1993, 05, 11));
        }

        [Fact]
        public void ConvertTo()
        {
            // Arrange
            var converter = new DateOnlyTypeConverter();

            // Act
            var result = converter.ConvertTo(new DateOnly(1993, 05, 11), typeof(string));

            // Assert
            result.Should().BeOfType<string>();
            result.Should().BeEquivalentTo("1993-05-11");
        }
    }
}
