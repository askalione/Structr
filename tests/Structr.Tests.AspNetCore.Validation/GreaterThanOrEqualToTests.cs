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
    public class GreaterThanOrEqualToTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public void Is_valid(int propertyValue)
        {
            // Act
            var result = Test(propertyValue, 1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(0, 1);

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be greater than or equal to Value2.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(0, 1, relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be greater than or equal to Value_2_display_name.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(0, 1, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(0, 1, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(1, null, true)]
        [InlineData(null, 2, true)]
        [InlineData(null, null, true)]
        public void Pass_null(object propertyValue, object relatedPropertyValue, bool isValid)
        {
            // Act
            var result = Test(propertyValue, relatedPropertyValue, passNull: true);

            // Assert
            (result == null).Should().Be(isValid);
        }

        private ValidationResult Test(object propertyValue,
            object relatedPropertyValue,
            string relatedPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null,
            bool? passNull = null) => TestValidation.TestIs<GreaterThanOrEqualToAttribute>(propertyValue,
                relatedPropertyValue,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType,
                passNull);
    }
}