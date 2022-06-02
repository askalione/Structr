
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Marks property as required when value of related property is NOT empty.
    /// </summary>
    public class RequiredIfNotEmptyAttribute : ContingentValidationAttribute
    {
        /// <summary>
        /// Marks property as required when value of related property is NOT empty.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        public RequiredIfNotEmptyAttribute(string relatedProperty)
            : base(relatedProperty) { }

        public override bool IsValid(object value, object relatedPropertyValue, object container)
        {
            if (PropertyIsEmpty(relatedPropertyValue) == false)
            {
                return PropertyIsEmpty(value) == false;
            }
            return true;
        }

        public override string DefaultErrorMessage => "{0} is required due to {1} not being empty.";
    }
}
