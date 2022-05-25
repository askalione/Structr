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
        public void RequiredIf(object value1, object value2, bool isValid)
        {
            // Act
            var result = Test(value1, value2);

            // Assert
            (result == null).Should().Be(isValid);
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
            var result = Test(null, false, dependentPropertyDisplayName: "Value 2 display name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 is required due to Value2 being equal to False.");
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

        private ValidationResult Test(object value1,
            object value2,
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null) => TestValidation.TestRequiredIf<RequiredIfFalseAttribute>(value1,
                value2,
                dependentPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType);
    }
}