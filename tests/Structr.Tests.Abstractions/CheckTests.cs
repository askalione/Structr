using FluentAssertions;
using Structr.Abstractions;
using System;
using Xunit;

namespace Structr.Tests.Abstractions
{
    public class CheckTests
    {
        [Theory]
        [InlineData("123", false)]
        [InlineData("123456", true)]
        [InlineData("123456789", false)]
        public void IsInRange_for_String(string value, bool expected)
        {
            // Act
            var result = Check.IsInRange(value, 4, 8);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("1980-01-01", false)]
        [InlineData("2000-05-30", true)]
        [InlineData("2023-05-09", false)]
        public void IsInRange_for_DateTime(string value, bool expected)
        {
            // Arrange
            var dateTimeValue = DateTime.Parse(value);

            // Act
            var result = Check.IsInRange(dateTimeValue, DateTime.Parse("1990-01-01"), DateTime.Parse("2022-12-31"));

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("123", false)]
        [InlineData("123456", true)]
        public void IsGreaterThan_for_String(string value, bool expected)
        {
            // Act
            var result = Check.IsGreaterThan(value, 4);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("1980-01-01", false)]
        [InlineData("2000-05-30", true)]
        public void IsGreaterThan_for_DateTime(string value, bool expected)
        {
            // Arrange
            var dateTimeValue = DateTime.Parse(value);

            // Act
            var result = Check.IsGreaterThan(dateTimeValue, DateTime.Parse("1990-01-01"));

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("123456", true)]
        [InlineData("123456789", false)]
        public void IsLessThan_for_String(string value, bool expected)
        {
            // Act
            var result = Check.IsLessThan(value, 8);

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData("2000-05-30", true)]
        [InlineData("2023-05-09", false)]
        public void IsLessThan_for_DateTime(string value, bool expected)
        {
            // Arrange
            var dateTimeValue = DateTime.Parse(value);

            // Act
            var result = Check.IsLessThan(dateTimeValue, DateTime.Parse("2022-12-31"));

            // Assert
            result.Should().Be(expected);
        }
    }
}
