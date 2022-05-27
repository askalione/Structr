
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfEmptyAttribute : ContingentValidationAttribute
    {
        /// <inheritdoc cref="RequiredIfAttribute.RequiredIfAttribute"/>
        public RequiredIfEmptyAttribute(string relatedProperty)
            : base(relatedProperty) { }

        public override bool IsValid(object value, object relatedPropertyValue, object container)
        {
            if (string.IsNullOrEmpty((relatedPropertyValue ?? string.Empty).ToString().Trim()))
            {
                return value != null && string.IsNullOrEmpty(value.ToString().Trim()) == false;
            }
            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} being empty.";
    }
}
