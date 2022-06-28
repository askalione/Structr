using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidationExceptionTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var validationResult = new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("First failure."),
                new ValidationFailure("Second failure.")
            });

            // Act
            var result = new ValidationException(validationResult);

            // Assert
            result.ValidationResult.Should().Equal(validationResult);
            result.Message.Should().Be($"First failure.{Environment.NewLine}Second failure.");
        }

        [Fact]
        public void Ctor_throws_when_validationResult_is_null()
        {
            // Act
            Action act = () => new ValidationException(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
