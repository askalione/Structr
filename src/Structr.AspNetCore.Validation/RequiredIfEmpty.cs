
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfEmptyAttribute : ContingentValidationAttribute
    {
        public RequiredIfEmptyAttribute(string relatedProperty)
            : base(relatedProperty) { }

        public override bool IsValid(object value, object relatedValue, object container)
        {
            if (string.IsNullOrEmpty((relatedValue ?? string.Empty).ToString().Trim()))
            {
                return value != null && string.IsNullOrEmpty(value.ToString().Trim()) == false;
            }
            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} being empty.";
    }
}
