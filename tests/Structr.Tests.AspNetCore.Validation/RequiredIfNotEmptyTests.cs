#nullable disable

using Xunit;
using FluentAssertions;
using Structr.Tests.AspNetCore.Validation.TestData;
using System.ComponentModel.DataAnnotations;
using System;
using Structr.Tests.AspNetCore.Validation.TestUtils;

namespace Structr.Tests.AspNetCore.Validation
{
    public class RequiredIfNotEmptyTests
    {
        [Theory]
        [InlineData(1, 1, true)]
        [InlineData(1, null, true)]
        [InlineData(null, 1, false)]
        [InlineData("", 1, false)]
        [InlineData(null, null, true)]
        [InlineData("", null, true)]
        [InlineData(null, "", true)]
        [InlineData(null, new int[] { }, true)]
        [InlineData(null, new int[] { 5, 6, 7 }, false)]
        public void RequiredIfNotEmpty(object propertyValue, object relatedPropertyValue, bool isValid)
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
            var result = Test(propertyValue, "abc");

            // Assert
            (result != null).Should().Be(requiredIsEmpty);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(null, 1);

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value2 not being empty.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(null, 1, relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value_2_display_name not being empty.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(null, 1, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(null, 1, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private ValidationResult Test(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRequiredIfNotEmpty(propertyValue,
                relatedPropertyValue,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}