using Structr.Abstractions.Exceptions;
using System;

namespace Structr.Validation
{
    public class ValidationException : AdHocException
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
