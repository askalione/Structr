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
    public class LessThanTests
    {
        [Fact]
        public void Is_valid()
        {
            // Act
            var result = Test(1, 2);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(1, 1);

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be less than Value2.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(1, 1, relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be less than Value_2_display_name.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(1, 1, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(1, 1, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        [Theory]
        [InlineData(2, 1, false)]
        [InlineData(2, null, true)]
        [InlineData(null, 1, true)]
        [InlineData(null, null, false)]
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
            bool? passNull = null) => TestValidation.TestIs<LessThanAttribute>(propertyValue,
                relatedPropertyValue,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType,
                passNull);
    }
}