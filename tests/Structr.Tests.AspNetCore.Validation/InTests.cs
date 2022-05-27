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
    public class InTests
    {
        private class InData : TheoryData<object, object, bool>
        {
            public InData()
            {
                Add(1, 1, true);
                Add(1, 2, false);
                Add(1, new int[] { 1, 2, 3 }, true);
                Add(1, new object[] { DateTime.Now, 1, "2" }, true);
                Add(1, new object[] { DateTime.Now, "1", "2" }, false);
                Add('1', "123", true);
                Add("123", '1', false);
                Add("123", "012345", false);
            }
        }
        [Theory]
        [ClassData(typeof(InData))]
        public void In(object propertyValue, object relatedPropertyValue, bool isValid)
        {
            // Act
            var result = Test(propertyValue, relatedPropertyValue);

            // Assert
            (result == null).Should().Be(isValid);
        }

        [Fact]
        public void Gives_standard_message()
        {
            // Act
            var result = Test(1, 2);

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be in Value2.");
        }

        [Fact]
        public void Gives_display_name_in_message()
        {
            // Act
            var result = Test(1, 2, relatedPropertyDisplayName: "Value_2_display_name");

            // Assert
            result.ErrorMessage.Should().Be("Value1 must be in Value_2_display_name.");
        }

        [Fact]
        public void Gives_custom_message()
        {
            // Act
            var result = Test(1, 2, errorMessage: "Custom error message.");

            // Assert
            result.ErrorMessage.Should().Be("Custom error message.");
        }

        [Fact]
        public void Gives_message_from_resource_Model()
        {
            // Act
            var result = Test(1, 2, errorMessageResourceName: "ErrorMessageFromResource", errorMessageResourceType: typeof(ErrorMessages));

            // Assert
            result.ErrorMessage.Should().Be(ErrorMessages.ErrorMessageFromResource);
        }

        private class PassNullData : TheoryData<object, object, bool>
        {
            public PassNullData()
            {
                Add(1, new int[] { 5, 6 }, false);
                Add(null, new int[] { 5, 6 }, true);
                Add(1, null, true);
                Add(null, null, true);
            }
        }
        [Theory]
        [ClassData(typeof(PassNullData))]
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
            bool? passNull = null) => TestValidation.TestIs<InAttribute>(propertyValue,
                relatedPropertyValue,
                relatedPropertyDisplayName,
                errorMessage,
                errorMessageResourceName,
                errorMessageResourceType,
                passNull);
    }
}