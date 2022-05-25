#nullable disable

using Xunit;
using FluentAssertions;
using Structr.AspNetCore.Validation;
using Structr.Tests.AspNetCore.Validation.TestData;
using System.ComponentModel.DataAnnotations;
using System;

namespace Structr.Tests.AspNetCore.Validation
{
    public class EqualToTests
    {
        private class TestModel
        {
            public int? Value1 { get; set; }
            public int? Value2 { get; set; }
        }

        [Fact]
        public void Is_valid()
        {
            // Act
            var result = TestValidation(1, 1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = TestValidation(1, 2);

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be equal to Value2.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = TestValidation(1, 2, dependentPropertyDisplayName: "Value 2 display name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be equal to Value 2 display name.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = TestValidation(1, 2, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = TestValidation(1, 2, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(1, null, true)]
        [InlineData(null, 2, true)]
        [InlineData(null, null, true)]
        public void Pass_null(int? value1, int? value2, bool isValid)
        {
            // Act
            var result = TestValidation(value1, value2, passNull: true);

            // Assert
            if (isValid)
            {
                result.Should().BeNull();
            }
            else
            {
                result.Should().NotBeNull();
            }
        }

        private ValidationResult TestValidation(int? value1,
            int? value2,
            string dependentPropertyDisplayName = null,
            string errorMessage = null,
            string errorMessageResourceName = null,
            Type errorMessageResourceType = null,
            bool? passNull = null)
        {
            var model = new TestModel { Value1 = value1, Value2 = value2 };

            var attribute = new EqualToAttribute(nameof(TestModel.Value2));

            if (dependentPropertyDisplayName != null)
            {
                attribute.DependentPropertyDisplayName = dependentPropertyDisplayName;
            }
            if (errorMessage != null)
            {
                attribute.ErrorMessage = errorMessage;
            }
            if (errorMessageResourceName != null)
            {
                attribute.ErrorMessageResourceName = errorMessageResourceName;
                attribute.ErrorMessageResourceType = errorMessageResourceType;
            }
            if (passNull != null)
            {
                attribute.PassOnNull = (bool)passNull;
            }

            var validationContext = new ValidationContext(model) { MemberName = nameof(TestModel.Value1) };

            return attribute.GetValidationResult(model.Value1, validationContext);
        }
    }
}