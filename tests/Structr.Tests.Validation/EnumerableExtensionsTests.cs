using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ToValidationResult()
        {
            // Arrange
            IEnumerable<ValidationFailure> validationFailures = new List<ValidationFailure>()
            {
                new ValidationFailure("First failure."),
                new ValidationFailure("Second failure.")
            };

            // Act
            var validationResult = validationFailures.ToValidationResult();

            // Assert
            validationResult.Should().SatisfyRespectively(
                first =>
                {
                    first.Message.Should().Be("First failure.");
                },
                second =>
                {
                    second.Message.Should().Be("Second failure.");
                });
        }

        [Fact]
        public void ToValidationResult_throws_when_failures_is_null()
        {
            // Arrange
            List<ValidationFailure>? validationFailures = null;

            // Act
            Action act = () => validationFailures.ToValidationResult();

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
