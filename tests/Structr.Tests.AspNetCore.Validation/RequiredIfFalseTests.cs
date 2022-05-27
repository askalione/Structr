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
    public class RequiredIfFalseTests
    {
        [Theory]
        [InlineData(1, false, true)]
        [InlineData(1, true, true)]
        [InlineData(null, false, false)]
        [InlineData(null, true, true)]
        [InlineData(null, null, true)]
        public void RequiredIfFalse(object propertyValue, object relatedPropertyValue, bool isValid)
        {
            // Act
            var result = Test(propertyValue, relatedPropertyValue);

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
            var result = Test(propertyValue, false);

            // Assert
            (result != null).Should().Be(requiredIsEmpty);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(null, false);

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value2 being equal to False.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(null, false, relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value_2_display_name being equal to False.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(null, false, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(null, false, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private ValidationResult Test(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRequiredIf<RequiredIfFalseAttribute>(propertyValue,
                relatedPropertyValue,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}