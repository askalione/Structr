using System;

namespace Structr.Validation
{
    /// <summary>
    /// Represents failures that occur during validation execution.
    /// </summary>
    public class ValidationException : Exception
    {
        /// <summary>
        /// The <see cref="ValidationResult"/> - list of <see cref="ValidationFailure"/>.
        /// </summary>
        public ValidationResult ValidationResult { get; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message => ValidationResult.ToString();

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationException"/>.
        /// </summary>
        /// <param name="validationResult">The <see cref="ValidationResult"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="validationResult"/> is <see langword="null"/>.</exception>
        public ValidationException(ValidationResult validationResult) : base()
        {
            if (validationResult == null)
            {
                throw new ArgumentNullException(nameof(validationResult));
            }

            ValidationResult = validationResult;
        }
    }
}
