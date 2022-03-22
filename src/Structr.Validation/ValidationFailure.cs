namespace Structr.Validation
{
    public class ValidationFailure
    {
        public string ParameterName { get; set; }
        public object ActualValue { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public ValidationFailureLevel Level { get; set; } = ValidationFailureLevel.Error;

        public ValidationFailure(string message)
        {
            Message = message;
        }

        public ValidationFailure(string parameterName, string message) : this(message)
        {
            ParameterName = parameterName;
        }

        public ValidationFailure(string parameterName, object actualValue, string message) : this(parameterName, message)
        {
            ActualValue = actualValue;
        }
    }
}
