using FluentAssertions;
using Structr.AspNetCore.TypeConverters;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.TypeConverters
{
    public class TimeOnlyTypeConverterTests
    {
        [Fact]
        public void ConvertFrom()
        {
            // Arrange
            var converter = new TimeOnlyTypeConverter();

            // Act
            var result = converter.ConvertFrom("11:15:30.0000000");

            // Assert
            result.Should().BeOfType<TimeOnly>();
            result.Should().BeEquivalentTo(new TimeOnly(11, 15, 30, 0));
        }

        [Fact]
        public void ConvertTo()
        {
            // Arrange
            var converter = new TimeOnlyTypeConverter();

            // Act
            var result = converter.ConvertTo(new TimeOnly(11, 15, 30, 0), typeof(string));

            // Assert
            result.Should().BeOfType<string>();
            result.Should().BeEquivalentTo("11:15:30.0000000");
        }
    }
}
