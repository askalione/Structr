using Structr.Validation;

namespace Structr.Tests.Validation
{
    public class ValidationResultTests
    {
        private IEnumerable<ValidationFailure> _validationFailures;
        private ValidationResult _validationResult;

        public ValidationResultTests()
        {
            _validationFailures = new List<ValidationFailure>()
            {
                new ValidationFailure("First failure."),
                new ValidationFailure("Second failure.")
            };
            _validationResult = _validationFailures.ToValidationResult();
        }

        [Fact]
        public void Ctor()
        {
            // Act
            var validationResult = new ValidationResult();

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Ctor_with_failures()
        {
            // Act
            var validationResult = new ValidationResult(_validationFailures);

            // Assert
            validationResult.Should()
                .BeEquivalentTo(new ValidationFailure[] { _validationFailures.First(), _validationFailures.Last() });
        }

        [Fact]
        public void Ctor_throws_when_failures_is_null()
        {
            // Act
            Action act = () => new ValidationResult(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void GetEnumerator()
        {
            // Act
            var enumerator = _validationResult.GetEnumerator();

            // Assert
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Message.Should().Be("First failure.");
            enumerator.MoveNext().Should().BeTrue();
            enumerator.Current.Message.Should().Be("Second failure.");
            enumerator.MoveNext().Should().BeFalse();
        }

        [Fact]
        public void ToStringTest()
        {
            // Act
            var result = _validationResult.ToString();

            // Assert
            result.Should().Be($"First failure.{Environment.NewLine}Second failure.");
        }

        [Fact]
        public void ToString_with_separator()
        {
            // Act
            var result = _validationResult.ToString("---");

            // Assert
            result.Should().Be($"First failure.---Second failure.");
        }

        [Fact]
        public void IsValid()
        {
            // Act
            var result = _validationResult.IsValid;

            // Assert
            result.Should().BeFalse();
        }
    }
}
