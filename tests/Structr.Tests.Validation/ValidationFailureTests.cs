using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidationFailureTests
    {
        [Fact]
        public void Ctor_with_message()
        {
            // Act
            var result = new ValidationFailure("Message");

            // Assert
            result.Message.Should().Be("Message");
        }

        [Fact]
        public void Ctor_with_message_and_parameterName()
        {
            // Act
            var result = new ValidationFailure("Id", "Message");

            // Assert
            result.Message.Should().Be("Message");
            result.ParameterName.Should().Be("Id");
        }

        [Fact]
        public void Ctor_with_message_and_parameterName_and_actualValue()
        {
            // Act
            var result = new ValidationFailure("Id", 15, "Message");

            // Assert
            result.Message.Should().Be("Message");
            result.ParameterName.Should().Be("Id");
            result.ActualValue.Should().Be(15);
        }
    }
}
