namespace Structr.Validation
{
    public interface IValidationFailure
    {
        string ParameterName { get; }
        object ActualValue { get; }
        string ErrorMessage { get; }
    }
}
