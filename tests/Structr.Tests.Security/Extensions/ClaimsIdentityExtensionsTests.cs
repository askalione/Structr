using Xunit;
using FluentAssertions;
using System;
using System.Security.Claims;
using Structr.Security.Extensions;

namespace Structr.Tests.Security.Extensions
{
    public class ClaimsIdentityExtensionsTests
    {
        [Fact]
        public void AddClaim()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            var result = claimsIdentity.AddClaim("TestClaimType", "TestClaimValue");

            // Assert
            result.Claims.Should().Contain(x => x.Type == "TestClaimType" && x.Value == "TestClaimValue");
        }

        [Fact]
        public void AddClaim_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.AddClaim("TestClaimType", "TestClaimValue");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Fact]
        public void AddClaim_throws_when_type_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.AddClaim("", "TestClaimValue");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void AddClaim_throws_when_value_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.AddClaim("TestClaimType", "");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*value*");
        }

        [Fact]
        public void SetClaim()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsIdentity.SetClaim("TestClaimType", "new TestClaimValue");

            // Assert
            result.Claims.Should().Contain(x => x.Type == "TestClaimType" && x.Value == "new TestClaimValue");
        }

        [Fact]
        public void SetClaim_adds_claim_when_none_of_type_existed()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("AnotherTestClaimType", "TestClaimValue"));

            // Act
            var result = claimsIdentity.SetClaim("TestClaimType", "new TestClaimValue");

            // Assert
            result.Claims.Should().Contain(x => x.Type == "TestClaimType" && x.Value == "new TestClaimValue");
        }

        [Fact]
        public void SetClaim_removes_when_value_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsIdentity.SetClaim("TestClaimType", null);

            // Assert
            result.Claims.Should().NotContain(x => x.Type == "TestClaimType");
        }

        [Fact]
        public void SetClaim_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.SetClaim("TestClaimType", "TestClaimValue");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Fact]
        public void SetClaim_throws_when_type_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.SetClaim("", "TestClaimValue");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("TestClaimType", "TestClaimValue"));

            // Act
            var result = claimsIdentity.SetClaims("TestClaimType", new string[] { "new TestClaimValue 1", "new TestClaimValue 2" });

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
        public void SetClaims_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.SetClaims("TestClaimType", new string[] { "new TestClaimValue 1", "new TestClaimValue 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Fact]
        public void SetClaims_throws_when_type_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.SetClaims("", new string[] { "new TestClaimValue 1", "new TestClaimValue 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims_throws_when_values_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.SetClaims("TestClaimType", null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*values*");
        }

        [Fact]
        public void RemoveClaims()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("TestClaimType_1", "TestClaimValue_11"));
            claimsIdentity.AddClaim(new Claim("TestClaimType_1", "TestClaimValue_12"));
            claimsIdentity.AddClaim(new Claim("TestClaimType_2", "TestClaimValue_21"));

            // Act
            var result = claimsIdentity.RemoveAllClaims("TestClaimType_1");

            // Assert
            result.Claims.Should().SatisfyRespectively(
                x =>
                {
                    x.Type.Should().Be("TestClaimType_2");
                    x.Value.Should().Be("TestClaimValue_21");
                });
        }

        [Fact]
        public void RemoveClaims_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.RemoveAllClaims("TestClaimType");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Fact]
        public void RemoveClaims_throws_when_type_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.RemoveAllClaims(null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }
    }
}