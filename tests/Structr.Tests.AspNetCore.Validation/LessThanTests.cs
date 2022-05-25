#nullable disable

using Xunit;
using FluentAssertions;
using Structr.AspNetCore.Validation;
using Structr.Tests.AspNetCore.Validation.TestData;
using System.ComponentModel.DataAnnotations;
using System;
using Structr.Tests.AspNetCore.Validation.TestUtils;

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
            var result = Test(1, 1, dependentPropertyDisplayName: "Value 2 display name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be less than Value 2 display name.");
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
        public void Pass_null(object value1, object value2, bool isValid)
        {
            // Act
            var result = Test(value1, value2, passNull: true);

            // Assert
            (result == null).Should().Be(isValid);
        }
        
        private ValidationResult Test(object value1,
            object value2,
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null,
            bool? passNull = null) => TestValidation.TestIs<LessThanAttribute>(value1,
                value2,
                dependentPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType,
                passNull);
    }
}