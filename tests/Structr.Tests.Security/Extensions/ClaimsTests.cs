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
        public void GetFirstValue()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claims = new List<Claim> {
                new Claim("TestClaimType", $"1{separator}5"),
                new Claim("TestClaimType", $"2{separator}7"),
                new Claim("AnotherTestClaimType", "ABC"),
            };

            // Act
            var result = claims.GetFirstValue<float>("TestClaimType");

            // Assert
            result.Should().Be(1.5F);
        }

        [Fact]
        public void GetFirstValue_throws_when_source_is_null()
        {
            // Arrange
            IEnumerable<Claim> source = null!;

            // Act
            Action act = () => source.GetFirstValue<float>("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact]
        public void GetFirstValue_throws_when_type_is_null()
        {
            // Arrange
            var claims = new List<Claim>();

            // Act
            Action act = () => claims.GetFirstValue<float>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void GetFirstValue_throws_when_cast_is_invalid()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claims = new List<Claim> {
                new Claim("TestClaimType", $"1{separator}5"),
                new Claim("TestClaimType", $"2{separator}7"),
                new Claim("AnotherTestClaimType", "ABC"),
            };

            // Act
            Action act = () => claims.GetFirstValue<bool>("TestClaimType");

            // Assert
            act.Should().Throw<InvalidCastException>().WithMessage("*claim*");
        }

        [Fact]
        public void FindAllValues()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claims = new List<Claim> {
                new Claim("TestClaimType", $"1{separator}5"),
                new Claim("TestClaimType", $"2{separator}7"),
                new Claim("AnotherTestClaimType", "ABC"),
            };

            // Act
            var result = claims.FindAllValues<float>("TestClaimType");

            // Assert
            result.Should().Contain(new [] { 1.5F, 2.7F });
        }

        [Fact]
        public void FindAllValues_throws_when_source_is_null()
        {
            // Arrange
            IEnumerable<Claim> source = null!;

            // Act
            Action act = () => source.FindAllValues<float>("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Fact]
        public void FindAllValues_throws_when_type_is_null()
        {
            // Arrange
            var claims = new List<Claim>();

            // Act
            Action act = () => claims.FindAllValues<float>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void FindAllValues_for_different_claim_types()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claims = new List<Claim> {
                new Claim("TestClaimType", $"1{separator}5"),
                new Claim("TestClaimType", $"true"),
                new Claim("AnotherTestClaimType", "ABC"),
            };

            // Act
            var result = claims.FindAllValues<float>("TestClaimType");

            // Assert
            result.Should().HaveCount(1);
            result.Should().ContainSingle(value => value == 1.5F);
        }

        [Fact]
        public void GetValue_for_one_claim()
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
        public void GetValue_for_one_claim_throws_when_claim_is_null()
        {
            // Arrange
            Claim claim = null!;

            // Act
            Action act = () => claim.GetValue<float>();

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*claim*");
        }

        [Fact]
        public void GetValue_for_one_claim_throws_when_cast_is_invalid()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claim = new Claim("TestClaimType", $"1{separator}5");

            // Act
            Action act = () => claim.GetValue<bool>();

            // Assert
            act.Should().Throw<InvalidCastException>().WithMessage("*claim*");
        }
    }
}