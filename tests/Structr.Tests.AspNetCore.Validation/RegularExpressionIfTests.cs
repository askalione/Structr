#nullable disable

using Xunit;
using FluentAssertions;
using Structr.Tests.AspNetCore.Validation.TestData;
using System.ComponentModel.DataAnnotations;
using System;
using Structr.Tests.AspNetCore.Validation.TestUtils;

namespace Structr.Tests.AspNetCore.Validation
{
    public class RegularExpressionIfTests
    {
        [Theory]
        [InlineData("Abc", 2, true)]
        [InlineData("Abc", 1, false)]
        [InlineData("Bb0", 2, true)]
        [InlineData("Bb0", 1, true)]
        public void RegularExpressionIf(object propertyValue, object relatedPropertyValue, bool isValid)
        {
            // Act
            var result = Test(propertyValue, relatedPropertyValue, 1, "[A-Z][a-z]\\d");

            // Assert
            (result == null).Should().Be(isValid);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(null, 1, 1, "[A-Z][a-z]\\d");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be in the format of [A-Z][a-z]\\d due to Value2 being equal to 1");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(null, 1, 1, "[A-Z][a-z]\\d", relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be in the format of [A-Z][a-z]\\d due to Value_2_display_name being equal to 1");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(null, 1, 1, "[A-Z][a-z]\\d", errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(null, 1, 1, "[A-Z][a-z]\\d", errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private ValidationResult Test(object propertyValue,
            object relatedPropertyValue,
            object relatedPropertyExpectedValue,
            string pattern,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRegularExpressionIf(propertyValue,
                relatedPropertyValue,
                relatedPropertyExpectedValue,
                pattern,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}