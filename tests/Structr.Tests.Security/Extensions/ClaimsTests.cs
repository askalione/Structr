using Xunit;
using FluentAssertions;
using System;
using System.Security.Claims;
using Structr.Security.Extensions;
using System.Collections.Generic;
using System.Globalization;

namespace Structr.Tests.Security.Extensions
{
    public class ClaimsTests
    {
        [Fact]
        public void GetValue()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claims = new List<Claim> {
                new Claim("TestClaimType", $"1{separator}5"),
                new Claim("TestClaimType", $"2{separator}7"),
                new Claim("AnotherTestClaimType", "ABC"),
            };

            // Act
            var result = claims.GetValue<float>("TestClaimType");

            // Assert
            result.Should().Be(1.5F);
        }

        [Fact]
        public void GetValue_throws_when_source_is_null()
        {
            // Arrange
            IEnumerable<Claim> source = null!;

            // Act
            Action act = () => source.GetValue<float>("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact]
        public void GetValue_throws_when_type_is_null()
        {
            // Arrange
            var claims = new List<Claim>();

            // Act
            Action act = () => claims.GetValue<float>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void GetValues()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claims = new List<Claim> {
                new Claim("TestClaimType", $"1{separator}5"),
                new Claim("TestClaimType", $"2{separator}7"),
                new Claim("AnotherTestClaimType", "ABC"),
            };

            // Act
            var result = claims.GetValues<float>("TestClaimType");

            // Assert
            result.Should().Contain(new [] { 1.5F, 2.7F });
        }

        [Fact]
        public void GetValues_throws_when_source_is_null()
        {
            // Arrange
            IEnumerable<Claim> source = null!;

            // Act
            Action act = () => source.GetValues<float>("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact]
        public void GetValues_throws_when_type_is_null()
        {
            // Arrange
            var claims = new List<Claim>();

            // Act
            Action act = () => claims.GetValues<float>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void GetValue_for_one_clame()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claim = new Claim("TestClaimType", $"1{separator}5");

            // Act
            var result = claim.GetValue<float>();

            // Assert
            result.Should().Be(1.5F);
        }

        [Fact]
        public void GetValue_for_one_clame_throws_when_clame_is_null()
        {
            // Arrange
            Claim claim = null!;

            // Act
            Action act = () => claim.GetValue<float>();

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*claim*");
        }
    }
}