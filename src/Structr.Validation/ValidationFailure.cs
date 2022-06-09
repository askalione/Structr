namespace Structr.Validation
{
    /// <summary>
    /// Represents an information about a failure that have occured during validation.
    /// </summary>
    public class ValidationFailure
    {
        /// <summary>
        /// The name of the parameter that caused the failure.
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// The value of the parameter that caused the failure.
        /// </summary>
        public object ActualValue { get; set; }

        /// <summary>
        /// The message that describes the failure.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The failure code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The <see cref="ValidationFailureLevel"/>.
        /// </summary>
        public ValidationFailureLevel Level { get; set; } = ValidationFailureLevel.Error;

        /// <inheritdoc cref="ValidationFailure(string, string)"/>
        public ValidationFailure(string message)
        {
            Message = message;
        }

        /// <inheritdoc cref="ValidationFailure(string, object, string)"/>
        public ValidationFailure(string parameterName, string message) : this(message)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationFailure"/>.
        /// </summary>
        /// <param name="parameterName">The name of the parameter that caused the failure.</param>
        /// <param name="actualValue">The value of the parameter that caused the failure.</param>
        /// <param name="message">The message that describes the failure.</param>
        public ValidationFailure(string parameterName, object actualValue, string message) : this(parameterName, message)
        {
            ActualValue = actualValue;
        }
    }
}
