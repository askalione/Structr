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
    public class RequiredIfNotRegExMatchTests
    {
        [Theory]
        [InlineData(null, "Abc", false)]
        [InlineData(null, "Bb0", true)]
        [InlineData("a", "Abc", true)]
        [InlineData("a", "Bb0", true)]
        public void RequiredIfNotRegExMatch(object propertyValue, object relatedPropertyValue, bool isValid)
        {
            // Act
            var result = Test(propertyValue, relatedPropertyValue, "[A-Z][a-z]\\d");

            // Assert
            (result == null).Should().Be(isValid);
        }

        [Theory]
        [InlineData(null, true)]
        [InlineData(1, false)]
        [InlineData("a", false)]
        [InlineData("", true)]
        [InlineData("   ", true)]
        [InlineData(new int[] { }, true)]
        [InlineData(new int[] { 5, 6, 7 }, false)]
        public void Could_identify_required_as_empty(object propertyValue, bool requiredIsEmpty)
        {
            // Act
            var result = Test(propertyValue, "Abcde", "[A-Z][a-z]\\d");

            // Assert
            (result != null).Should().Be(requiredIsEmpty);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(null, "Abc", "[A-Z][a-z]\\d");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value2 being not a match to [A-Z][a-z]\\d.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(null, "Abc", "[A-Z][a-z]\\d", relatedPropertyDisplayName: "Value 2 display name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value 2 display name being not a match to [A-Z][a-z]\\d.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(null, "Abc", "[A-Z][a-z]\\d", errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(null, "Abc", "[A-Z][a-z]\\d", errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private ValidationResult Test(object propertyValue,
            object relatedPropertyValue,
            object pattern,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRequiredIf<RequiredIfNotRegExMatchAttribute>(propertyValue,
                relatedPropertyValue,
                pattern,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}