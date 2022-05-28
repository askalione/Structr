using Xunit;
using FluentAssertions;
using System;
using System.Security.Claims;
using Structr.Security.Extensions;
using System.Globalization;

namespace Structr.Tests.Security.Extensions
{
    public class ClaimsPrincipalExtensionsTests
    {
        [Fact]
        public void SetClaim()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsPrincipal.SetClaim("TestClaimType", "new TestClaimValue");

            // Assert
            result.Claims.Should().Contain(x => x.Type == "TestClaimType" && x.Value == "new TestClaimValue");
        }

        [Fact]
        public void SetClaim_adds_claim_when_none_of_type_existed()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal(("AnotherTestClaimType", "TestClaimValue"));

            // Act
            var result = claimsPrincipal.SetClaim("TestClaimType", "new TestClaimValue");

            // Assert
            result.Claims.Should().Contain(x => x.Type == "TestClaimType" && x.Value == "new TestClaimValue");
        }

        [Fact]
        public void SetClaim_removes_when_value_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsPrincipal.SetClaim("TestClaimType", null);

            // Assert
            result.Claims.Should().NotContain(x => x.Type == "TestClaimType");
        }

        [Fact]
        public void SetClaim_throws_when_principal_is_null()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null!;

            // Act
            Action act = () => claimsPrincipal.SetClaim("TestClaimType", "TestClaimValue");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*principal*");
        }

        [Fact]
        public void SetClaim_throws_when_type_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.SetClaim(null, "TestClaimValue");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsPrincipal.SetClaims("TestClaimType", new string[] { "new TestClaimValue 1", "new TestClaimValue 2" });

            // Assert
            result.Claims.Should().SatisfyRespectively(
                x =>
                {
                    x.Type.Should().Be("TestClaimType");
                    x.Value.Should().Be("new TestClaimValue 1");
                },
                x =>
                {
                    x.Type.Should().Be("TestClaimType");
                    x.Value.Should().Be("new TestClaimValue 2");
                });
        }

        [Fact]
        public void SetClaims_throws_when_principal_is_null()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null!;

            // Act
            Action act = () => claimsPrincipal.SetClaims("TestClaimType", new string[] { "new TestClaimValue 1", "new TestClaimValue 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*principal*");
        }

        [Fact]
        public void SetClaims_throws_when_type_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.SetClaims("", new string[] { "new TestClaimValue 1", "new TestClaimValue 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims_throws_when_values_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.SetClaims("TestClaimType", null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*values*");
        }

        [Fact]
        public void RemoveClaims()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType_1", "TestClaimValue_11"),
                ("TestClaimType_1", "TestClaimValue_12"),
                ("TestClaimType_2", "TestClaimValue_21"));

            // Act
            var result = claimsPrincipal.RemoveClaims("TestClaimType_1");

            // Assert
            result.Claims.Should().SatisfyRespectively(
                x =>
                {
                    x.Type.Should().Be("TestClaimType_2");
                    x.Value.Should().Be("TestClaimValue_21");
                });
        }

        [Fact]
        public void RemoveClaims_throws_when_principal_is_null()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null!;

            // Act
            Action act = () => claimsPrincipal.RemoveClaims("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*principal*");
        }

        [Fact]
        public void RemoveClaims_throws_when_type_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.RemoveClaims(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void GetClaim()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType", $"1{separator}5"));

            // Act
            var result = claimsPrincipal.GetClaim<float>("TestClaimType");

            // Assert
            result.Should().Be(1.5F);
        }

        [Fact]
        public void GetClaim_throws_when_principal_is_null()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null!;

            // Act
            Action act = () => claimsPrincipal.GetClaim<float>("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*principal*");
        }

        [Fact]
        public void GetClaim_throws_when_type_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.GetClaim<float>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void GetClaims()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType", $"1{separator}5"),
                ("TestClaimType", $"2{separator}7"));

            // Act
            var result = claimsPrincipal.GetClaims<float>("TestClaimType");

            // Assert
            result.Should().BeEquivalentTo(new[] { 1.5F, 2.7F });
        }

        [Fact]
        public void GetClaims_throws_when_principal_is_null()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null!;

            // Act
            Action act = () => claimsPrincipal.GetClaims<float>("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*principal*");
        }

        [Fact]
        public void GetClaims_throws_when_type_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.GetClaims<float>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [InlineData("TestClaimType", true)]
        [InlineData("AnotherTestClaimType", false)]
        public void HasClaim(string clameType, bool expected)
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal(("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsPrincipal.HasClaim(clameType);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void HasClaim_throws_when_principal_is_null()
        {
            // Arrange
            ClaimsPrincipal claimsPrincipal = null!;

            // Act
            Action act = () => claimsPrincipal.HasClaim("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*principal*");
        }

        [Fact]
        public void HasClaim_throws_when_type_is_null()
        {
            // Arrange
            var claimsPrincipal = GetClaimsPrincipal();

            // Act
            Action act = () => claimsPrincipal.HasClaim("");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        private ClaimsPrincipal GetClaimsPrincipal(params (string Type, string Value)[] claims)
        {
            var claimsIdentity = new ClaimsIdentity();
            foreach (var claim in claims)
            {
                claimsIdentity.AddClaim(new Claim(claim.Type, claim.Value));
            }
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }
    }
}