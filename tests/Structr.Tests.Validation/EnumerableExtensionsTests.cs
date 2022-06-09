using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ToValidationResult()
        {
            // Arrange
            var validationFailure1 = new ValidationFailure("First failure.");
            var validationFailure2 = new ValidationFailure("Second failure.");
            IEnumerable<ValidationFailure> validationFailures = new List<ValidationFailure>()
            {
                validationFailure1,
                validationFailure2
            };

            // Act
            var validationResult = validationFailures.ToValidationResult();

            // Assert
            validationResult.Should().BeEquivalentTo(new ValidationFailure[] { validationFailure1, validationFailure2 });
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
