using Xunit;
using FluentAssertions;
using Structr.AspNetCore.Validation;
using Structr.Tests.AspNetCore.Validation.TestUtils;
using Structr.Tests.AspNetCore.Validation.TestData;

namespace Structr.Tests.AspNetCore.Validation
{
    public class EqualToTests
    {
        private class Basic_Model
        {
            [EqualTo("Value2")]
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }
        [Fact]
        public void Is_valid()
        {
            // Arrange
            var model = new Basic_Model { Value1 = 1, Value2 = 1 };

            // Act
            var isValid = TestValidator.TryValidateObject(model, out _);

            // Assert
            isValid.Should().BeTrue();
        }
        [Fact]
        public void Gives_standard_message()
        {
            // Arrange
            var model = new Basic_Model { Value1 = 1, Value2 = 2 };

            // Act
            var isValid = TestValidator.TryValidateObject(model, out var results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(x => x.ErrorMessage == "Value1 must be equal to Value2.");
        }

        private class Gives_display_name_in_message_Model
        {
            [EqualTo("Value2", DependentPropertyDisplayName = "Value 2 display name")]
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }
        [Fact]
        public void Gives_display_name_in_message()
        {
            // Arrange
            var model = new Gives_display_name_in_message_Model { Value1 = 1, Value2 = 2 };

            // Act
            var isValid = TestValidator.TryValidateObject(model, out var results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(x => x.ErrorMessage == "Value1 must be equal to Value 2 display name.");
        }

        private class Gives_custom_message_Model
        {
            [EqualTo("Value2", ErrorMessage = "Custom error message.")]
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }
        [Fact]
        public void Gives_custom_message()
        {
            // Arrange
            var model = new Gives_custom_message_Model { Value1 = 1, Value2 = 2 };

            // Act
            var isValid = TestValidator.TryValidateObject(model, out var results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(x => x.ErrorMessage == "Custom error message.");
        }

        private class Gives_message_from_resource_Model
        {
            [EqualTo("Value2", ErrorMessageResourceName = "ErrorMessageFromResource", ErrorMessageResourceType = typeof(ErrorMessages))]
            public int Value1 { get; set; }
            public int Value2 { get; set; }
        }
        [Fact]
        public void Gives_message_from_resource()
        {
            // Arrange
            var model = new Gives_message_from_resource_Model { Value1 = 1, Value2 = 2 };

            // Act
            var isValid = TestValidator.TryValidateObject(model, out var results);

            // Assert
            isValid.Should().BeFalse();
            results.Should().Contain(x => x.ErrorMessage == ErrorMessages.ErrorMessageFromResource);
        }

        private class Pass_null_Model
        {
            [EqualTo("Value2", PassOnNull = true)]
            public int? Value1 { get; set; }
            public int? Value2 { get; set; }
        }
        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(1, null, true)]
        [InlineData(null, 2, true)]
        [InlineData(null, null, true)]
        public void Pass_null(int? value1, int? value2, bool expected)
        {
            // Arrange
            var model = new Pass_null_Model { Value1 = value1, Value2 = value2 };

            // Act
            var isValid = TestValidator.TryValidateObject(model, out _);

            // Assert
            isValid.Should().Be(expected);
        }
    }
}