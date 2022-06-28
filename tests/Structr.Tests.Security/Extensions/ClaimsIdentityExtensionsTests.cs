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
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.SetClaim(type, "Value");

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
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.SetClaims(type, new string[] { "new Value 1", "new Value 2" });

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Fact]
        public void SetClaims_throws_when_values_are_null()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

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
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.RemoveAllClaims(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [InlineData("Type_2", "Value_21")]
        [InlineData("tYPE_2", "Value_21")]
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
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

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
                Add("Type_2", new string[] { "Value_21", "Value_22", "Value_22" });
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
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.FindAllValues(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [InlineData("Type_1", 1)]
        [InlineData("Type_2", 3)]
        [InlineData("tYPE_2", 3)]
        public void GetFirstValue(string type, int expected)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "1"));
            claimsIdentity.AddClaim(new Claim("Type_2", "3"));
            claimsIdentity.AddClaim(new Claim("Type_2", "4"));

            // Act
            var result = claimsIdentity.GetFirstValue<int>(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void GetFirstValue_throws_when_nothing_found()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.GetFirstValue<int>("Type_2");

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Claim with type \"Type_2\" not found.");
        }

        [Fact]
        public void GetFirstValue_throws_when_cast_is_invalid()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_21"));
            claimsIdentity.AddClaim(new Claim("Type_2", "Value_22"));

            // Act
            Action act = () => claimsIdentity.GetFirstValue<int>("Type_2");

            // Assert
            act.Should().ThrowExactly<InvalidCastException>()
                .WithMessage("Claim value \"Value_21\" cast to \"Int32\" is not valid.");
        }

        [Theory]
        [InlineData("", 0, false)]
        [InlineData("abc", 0, false)]
        [InlineData("3", 3, true)]
        public void TryGetFirstValue(string val, int expectedVal, bool expected)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "1"));
            claimsIdentity.AddClaim(new Claim("Type_2", val));
            claimsIdentity.AddClaim(new Claim("Type_2", "4"));

            // Act
            var result = claimsIdentity.TryGetFirstValue("Type_2", out int parsedVal);

            // Assert
            result.Should().Be(expected);
            parsedVal.Should().Be(expectedVal);
        }

        [Fact]
        public void TryGetFirstValue_when_type_not_found()
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            var result = claimsIdentity.TryGetFirstValue("Type_2", out int parsedVal);

            // Assert
            result.Should().BeFalse();
            parsedVal.Should().Be(0);
        }

        [Theory]
        [ClassData(typeof(FindAllValuesGenericTheoryData))]
        public void FindAllValuesGeneric(string type, IEnumerable<int> expected)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "1"));
            claimsIdentity.AddClaim(new Claim("Type_2", "3"));
            claimsIdentity.AddClaim(new Claim("tYPE_2", "4"));
            claimsIdentity.AddClaim(new Claim("tYPE_2", "4"));
            claimsIdentity.AddClaim(new Claim("Type_2", "abc"));

            // Act
            var result = claimsIdentity.FindAllValues<int>(type);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        private class FindAllValuesGenericTheoryData : TheoryData<string, IEnumerable<int>>
        {
            public FindAllValuesGenericTheoryData()
            {
                Add("Type_2", new int[] { 3, 4, 4 });
                Add("Type_3", new int[] { });
            }
        }

        [Fact]
        public void FindAllValuesGeneric_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.FindAllValues<int>("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindAllValuesGeneric_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.FindAllValues<int>(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [InlineData("Type_1", true)]
        [InlineData("Type_2", true)]
        [InlineData("tYPE_2", true)]
        [InlineData("Type_3", false)]
        public void HasClaim(string type, bool expected)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "1"));
            claimsIdentity.AddClaim(new Claim("Type_2", "3"));
            claimsIdentity.AddClaim(new Claim("Type_2", "4"));

            // Act
            var result = claimsIdentity.HasClaim(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void HasClaim_throws_when_identity_is_null()
        {
            // Arrange
            ClaimsIdentity claimsIdentity = null!;

            // Act
            Action act = () => claimsIdentity.HasClaim("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*identity*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void HasClaim_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("Type_1", "Value_11"));

            // Act
            Action act = () => claimsIdentity.HasClaim(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }
    }
}