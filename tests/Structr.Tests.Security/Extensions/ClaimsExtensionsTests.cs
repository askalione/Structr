using FluentAssertions;
using Structr.Security.Extensions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Structr.Tests.Security.Extensions
{
    public class ClaimsExtensionsTests
    {
        [Theory]
        [InlineData("Type_2", "Type_2", "Value_21")]
        [InlineData("Type_2", "tYPE_2", "Value_21")]
        [InlineData("Type_3", null, null)]
        public void FindFirst(string type, string expectedType, string expectedValue)
        {
            // Arrange
            var claims = new List<Claim> {
                new Claim("Type_1", "Value_11"),
                new Claim("Type_2", "Value_21"),
                new Claim("Type_2", "Value_22")
            };

            // Act
            var result = claims.FindFirst(type);

            // Assert
            result?.Type.ToUpper().Should().Be(expectedType.ToUpper());
            result?.Value.Should().Be(expectedValue);
        }

        [Fact]
        public void FindFirst_throws_when_source_is_null()
        {
            // Arrange
            List<Claim> claims = null!;

            // Act
            Action act = () => claims.FindFirst("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindFirst_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            Action act = () => claims.FindFirst(type);

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
            var claims = new List<Claim> {
                new Claim("Type_1", "Value_11"),
                new Claim("Type_2", "Value_21"),
                new Claim("Type_2", "Value_22")
            };

            // Act
            var result = claims.FindFirstValue(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void FindFirstValue_throws_when_source_is_null()
        {
            // Arrange
            List<Claim> claims = null!;

            // Act
            Action act = () => claims.FindFirstValue("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindFirstValue_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            Action act = () => claims.FindFirstValue(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }

        [Theory]
        [ClassData(typeof(FindAllValuesTheoryData))]
        public void FindAllValues(string type, IEnumerable<string> expected)
        {
            // Arrange
            var claims = new List<Claim> {
                new Claim("Type_1", "Value_11"),
                new Claim("Type_2", "Value_21"),
                new Claim("Type_2", "Value_22"),
                new Claim("Type_2", "Value_22")
            };

            // Act
            var result = claims.FindAllValues(type);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        private class FindAllValuesTheoryData : TheoryData<string, IEnumerable<string>>
        {
            public FindAllValuesTheoryData()
            {
                Add("Type_2", new string[] { "Value_21", "Value_22", "Value_22" });
                Add("tYPE_2", new string[] { "Value_21", "Value_22", "Value_22" });
                Add("Type_3", new string[] { });
            }
        }

        [Fact]
        public void FindAllValues_throws_when_source_is_null()
        {
            // Arrange
            List<Claim> claims = null!;

            // Act
            Action act = () => claims.FindAllValues("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindAllValues_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            Action act = () => claims.FindAllValues(type);

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
            var claims = new List<Claim> {
                new Claim("Type_1", "1"),
                new Claim("Type_2", "3"),
                new Claim("Type_2", "4")
            };

            // Act
            var result = claims.GetFirstValue<int>(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void GetFirstValue_throws_when_nothing_found()
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            Action act = () => claims.GetFirstValue<int>("Type_2");

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Claim with type \"Type_2\" not found.");
        }

        [Fact]
        public void GetFirstValue_throws_when_cast_is_invalid()
        {
            // Arrange
            var claims = new List<Claim> {
                new Claim("Type_1", "Value_11"),
                new Claim("Type_2", "Value_21"),
                new Claim("Type_2", "Value_22")
            };

            // Act
            Action act = () => claims.GetFirstValue<int>("Type_2");

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
            var claims = new List<Claim> {
                new Claim("Type_1", "1"),
                new Claim("Type_2", val),
                new Claim("Type_2", "4")
            };

            // Act
            var result = claims.TryGetFirstValue("Type_2", out int parsedVal);

            // Assert
            result.Should().Be(expected);
            parsedVal.Should().Be(expectedVal);
        }

        [Fact]
        public void TryGetFirstValue_when_type_not_found()
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            var result = claims.TryGetFirstValue("Type_2", out int parsedVal);

            // Assert
            result.Should().BeFalse();
            parsedVal.Should().Be(0);
        }

        [Theory]
        [ClassData(typeof(FindAllValuesGenericTheoryData))]
        public void FindAllValuesGeneric(string type, IEnumerable<int> expected)
        {
            // Arrange
            var claims = new List<Claim> {
                new Claim("Type_1", "1"),
                new Claim("Type_2", "3"),
                new Claim("tYPE_2", "4"),
                new Claim("tYPE_2", "4"),
                new Claim("Type_2", "abc"),
            };

            // Act
            var result = claims.FindAllValues<int>(type);

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
        public void FindAllValuesGeneric_throws_when_source_is_null()
        {
            // Arrange
            List<Claim> claims = null!;

            // Act
            Action act = () => claims.FindAllValues<int>("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FindAllValuesGeneric_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            Action act = () => claims.FindAllValues<int>(type);

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
            var claims = new List<Claim> {
                new Claim("Type_1", "1"),
                new Claim("Type_2", "3"),
                new Claim("Type_2", "4"),
            };

            // Act
            var result = claims.HasClaim(type);

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void HasClaim_throws_when_source_is_null()
        {
            // Arrange
            List<Claim> claims = null!;

            // Act
            Action act = () => claims.HasClaim("Type");

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*source*");
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void HasClaim_throws_when_type_is_null_or_empty(string type)
        {
            // Arrange
            var claims = new List<Claim> { new Claim("Type_1", "Value_11") };

            // Act
            Action act = () => claims.HasClaim(type);

            // Assert
            act.Should().Throw<ArgumentNullException>().WithMessage("*type*");
        }
    }
}