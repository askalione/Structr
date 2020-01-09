using System;

namespace Structr.Validation
{
    public class ValidationFailure : IValidationFailure
    {
        public string ParameterName { get; }
        public object ActualValue { get; }
        public string ErrorMessage { get; }

        public ValidationFailure(string parameterName, object actualValue, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
                throw new ArgumentNullException(nameof(errorMessage));

            ParameterName = parameterName;
            ActualValue = actualValue;
            ErrorMessage = errorMessage;
        }

        public ValidationFailure(string errorMessage) : this(null, null, errorMessage)
        {
        }
    }
}
