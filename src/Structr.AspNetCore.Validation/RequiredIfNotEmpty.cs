
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfNotEmptyAttribute : ContingentValidationAttribute
    {
        public RequiredIfNotEmptyAttribute(string dependentProperty)
            : base(dependentProperty) { }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (string.IsNullOrEmpty((dependentValue ?? string.Empty).ToString().Trim()) == false)
            {
                return value != null && string.IsNullOrEmpty(value.ToString().Trim()) == false;
            }

            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} not being empty.";
    }
}
