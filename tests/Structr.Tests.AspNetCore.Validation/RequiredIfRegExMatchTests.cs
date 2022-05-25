#nullable disable

using Xunit;
using FluentAssertions;
using Structr.Tests.AspNetCore.Validation.TestData;
using System.ComponentModel.DataAnnotations;
using System;
using Structr.Tests.AspNetCore.Validation.TestUtils;
using Structr.AspNetCore.Validation;

namespace Structr.Tests.AspNetCore.Validation
{
    public class RequiredIfRegExMatchTests
    {
        [Theory]
        [InlineData(null, "Abc", true)]
        [InlineData(null, "Bb0", false)]
        [InlineData("a", "Abc", true)]
        [InlineData("a", "Bb0", true)]
        public void RequiredIfRegExMatch(object value1, object value2, bool isValid)
        {
            // Act
            var result = Test(value1, value2, "[A-Z][a-z]\\d");

            // Assert
            (result == null).Should().Be(isValid);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(null, "Bb0", "[A-Z][a-z]\\d");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value2 being a match to [A-Z][a-z]\\d.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(null, "Bb0", "[A-Z][a-z]\\d", dependentPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value_2_display_name being a match to [A-Z][a-z]\\d.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(null, "Bb0", "[A-Z][a-z]\\d", errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(null, "Bb0", "[A-Z][a-z]\\d", errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private ValidationResult Test(object value1,
            object value2,
            object pattern,
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRequiredIf<RequiredIfRegExMatchAttribute>(value1,
                value2,
                pattern,
                dependentPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}