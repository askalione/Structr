using FluentAssertions;
using Structr.Abstractions.Extensions;
using System;
using System.Threading;
using Xunit;

namespace Structr.Tests.Abstractions.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Theory]
        [InlineData(false, "09/25/2008")]
        [InlineData(true, "------")]
        public void ToShortDateString(bool isNull, string expected)
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            DateTime? dateTime = isNull ? null : new DateTime(2008, 09, 25, 11, 35, 52);

            // Act
            var result = dateTime.ToShortDateString("------");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(false, "Thursday, 25 September 2008")]
        [InlineData(true, "------")]
        public void ToLongDateString(bool isNull, string expected)
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            DateTime? dateTime = isNull ? null : new DateTime(2008, 09, 25, 11, 35, 52);

            // Act
            var result = dateTime.ToLongDateString("------");

            // Assert
            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(false, "25-09-2008 11:35:52")]
        [InlineData(true, "------")]
        public void ToStringTest(bool isNull, string expected)
        {
            // Arrange
            DateTime? dateTime = isNull ? null : new DateTime(2008, 09, 25, 11, 35, 52);

            // Act
            var result = dateTime.ToString("dd-MM-yyyy hh:mm:ss", "------");

            // Assert
            result.Should().Be(expected);
        }
    }
}