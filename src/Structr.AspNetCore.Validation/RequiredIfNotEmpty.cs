
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfNotEmptyAttribute : ContingentValidationAttribute
    {
        /// <inheritdoc cref="RequiredIfAttribute.RequiredIfAttribute"/>
        public RequiredIfNotEmptyAttribute(string relatedProperty)
            : base(relatedProperty) { }

        public override bool IsValid(object value, object relatedPropertyValue, object container)
        {
            if (string.IsNullOrEmpty((relatedPropertyValue ?? string.Empty).ToString().Trim()) == false)
            {
                return value != null && string.IsNullOrEmpty(value.ToString().Trim()) == false;
            }
            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} not being empty.";
    }
}
