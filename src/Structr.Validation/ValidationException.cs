using System;

namespace Structr.Validation
{
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; }
        public override string Message => ValidationResult.ToString();

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
