#nullable disable

using FluentAssertions;
using Structr.AspNetCore.Validation;
using Structr.Tests.AspNetCore.Validation.TestData;
using Structr.Tests.AspNetCore.Validation.TestUtils;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Structr.Tests.AspNetCore.Validation
{
    public class RequiredIfNotTests
    {
        [Theory]
        [InlineData(null, 2, false)]
        [InlineData(null, 1, true)]
        [InlineData("a", 2, true)]
        [InlineData("a", 1, true)]
        public void RequiredIfNot(object propertyValue, object relatedPropertyValue, bool isValid)
        {
            // Act
            var result = Test(propertyValue, relatedPropertyValue, 1);

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
            var result = Test(propertyValue, 1, 2);

            // Assert
            (result != null).Should().Be(requiredIsEmpty);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(null, 1, 2);

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value2 being not equal to 2.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(null, 1, 2, relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value_2_display_name being not equal to 2.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(null, 1, 2, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(null, 1, 2, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private ValidationResult Test(object propertyValue,
            object relatedPropertyValue,
            object relatedPropertyExpectedValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRequiredIf<RequiredIfNotAttribute>(propertyValue,
                relatedPropertyValue,
                relatedPropertyExpectedValue,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}