using FluentAssertions;
using Structr.Security.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Structr.Tests.Security.Extensions
{
    public class ClaimsIdentityExtensionsTests
    {
        [Fact]
        public void AddClaim()
        {
            // Act
            var result = new ClaimsIdentity().AddClaim("Type", "Value");

            // Assert
            result.Claims.Should().Contain(x => x.Type == "Type" && x.Value == "Value");
        }

        [Fact]
        public void AddClaim_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.AddClaim("Type", "Value");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Fact]
        public void AddClaim_throws_when_type_is_null()
        {
            // Act
            Action act = () => new ClaimsIdentity().AddClaim("", "Value");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void AddClaim_throws_when_value_is_null()
        {
            // Act
            Action act = () => new ClaimsIdentity().AddClaim("Type", "");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*value*");
        }

        [Fact]
        public void SetClaim()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type", "Value"));

            // Act
            var result = claimsIdentity.SetClaim("Type", "new Value");

            // Assert
            result.Claims.Should().Satisfy(x => x.Type == "Type" && x.Value == "new Value");
        }

        [Fact]
        public void SetClaim_adds_claim_when_none_of_type_existed()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("AnotherType", "Value"));

            // Act
            var result = claimsIdentity.SetClaim("Type", "new Value");

            // Assert
            result.Claims.Should().Satisfy(x => x.Type == "AnotherType" && x.Value == "Value",
                x => x.Type == "Type" && x.Value == "new Value");
        }

        [Fact]
        public void SetClaim_removes_when_value_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type", "Value"));

            // Act
            var result = claimsIdentity.SetClaim("Type", null);

            // Assert
            result.Claims.Should().BeEmpty();
        }

        [Fact]
        public void SetClaim_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.SetClaim("Type", "Value");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SetClaim_throws_when_type_is_null_or_empty(string type)
        {
            // Act
            Action act = () => new ClaimsIdentity().SetClaim(type, "Value");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type", "Value"));

            // Act
            var result = claimsIdentity.SetClaims("Type", new string[] {
                "new Value 1",
                "new Value 2",
                "new Value 2",
                "new value 2",
            });

            // Assert
            result.Claims.Should().SatisfyRespectively(
                x =>
                {
                    x.Type.Should().Be("Type");
                    x.Value.Should().Be("new Value 1");
                },
                x =>
                {
                    x.Type.Should().Be("Type");
                    x.Value.Should().Be("new Value 2");
                },
                x =>
                {
                    x.Type.Should().Be("Type");
                    x.Value.Should().Be("new value 2");
                });
        }

        [Fact]
        public void SetClaims_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.SetClaims("Type", new string[] { "new Value 1", "new Value 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SetClaims_throws_when_type_is_null_or_empty(string type)
        {
            // Act
            Action act = () => new ClaimsIdentity().SetClaims(type, new string[] { "new Value 1", "new Value 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims_throws_when_values_is_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.SetClaims("Type", null);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*values*");
        }

        [Fact]
        public void RemoveClaims()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_12"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_21"));

            // Act
            var result = claimsIdentity.RemoveAllClaims("Type_1");

            // Assert
            result.Claims.Should().Satisfy(x => x.Type == "Type_2" && x.Value == "Value_21");
        }

        [Fact]
        public void RemoveClaims_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.RemoveAllClaims("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RemoveClaims_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.RemoveAllClaims(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [InlineData("Type_2", "Value_21")]
        [InlineData("Type_3", null)]
        public void FindFirstValue(string type, string expected)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_21"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_22"));

            // Act
            var result = claimsIdentity.FindFirstValue(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void FindFirstValue_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.FindFirstValue("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindFirstValue_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.FindFirstValue(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [ClassData(typeof(FindAllValuesTheoryData))]
        public void FindAllValues(string type, IEnumerable<string> expected)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_21"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_22"));

            // Act
            var result = claimsIdentity.FindAllValues(type);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        private class FindAllValuesTheoryData : TheoryData<string, IEnumerable<string>>
        {
            public FindAllValuesTheoryData()
            {
                Add("Type_2", new string[] { "Value_21", "Value_22" });
                Add("Type_3", new string[] { });
            }
        }

        [Fact]
        public void FindAllValues_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.FindAllValues("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindAllValues_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.FindAllValues(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void GetFirstValue()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_21"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_22"));

            // Act
            var result = claimsIdentity.FindFirstValue("Type_2");

            // Assert
            result.Should().Be("Value_21");
        }

        [Fact]
        public void GetFirstValue_throws_when_nothing_found()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();

            // Act
            Action act = () => claimsIdentity.GetFirstValue<int>("Type_2");

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Claim with type \"Type_2\" not found.");
        }
    }
}