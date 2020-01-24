using System;

namespace Structr.Validation
{
    public class ValidationException : Exception
    {
        public IValidationResult ValidationResult { get; }
        public override string Message => ValidationResult.ToString();

        public ValidationException(IValidationResult validationResult) : base()
        {
            if (validationResult == null)
                throw new ArgumentNullException(nameof(validationResult));

            ValidationResult = validationResult;
        }
    }
}
