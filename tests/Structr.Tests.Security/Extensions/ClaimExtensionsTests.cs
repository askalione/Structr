using FluentAssertions;
using Structr.Security.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Xunit;

namespace Structr.Tests.Security.Extensions
{
    public class ClaimExtensionsTests
    {
        [Fact]
        public void GetValue()
        {
            // Arrange
            var claim = new Claim("Type_1", "3");

            // Act
            var result = claim.GetValue<int>();

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        public void GetValue_throws_when_cast_is_invalid()
        {
            // Arrange
            var claim = new Claim("Type_1", "abc");

            // Act
            Action act = () => claim.GetValue<int>();

            // Assert
            act.Should().ThrowExactly<InvalidCastException>();
        }

        [Fact]
        public void GetValue_throws_when_claim_is_null()
        {
            // Arrange
            Claim claim = null!;

            // Act
            Action act = () => claim.GetValue<int>();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>().WithMessage("*claim*");
        }

        [Theory]
        [InlineData("", 0, false)]
        [InlineData("abc", 0, false)]
        [InlineData("3", 3, true)]
        public void TryGetValue(string val, int expectedVal, bool expected)
        {
            // Arrange
            var claim = new Claim("Type_1", val);

            // Act
            var result = claim.TryGetValue(out int parsedVal);

            // Assert
            result.Should().Be(expected);
            parsedVal.Should().Be(expectedVal);
        }
    }
}