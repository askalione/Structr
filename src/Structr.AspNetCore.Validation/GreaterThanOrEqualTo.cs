
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be greater or equal to a value of specified related property.
    /// </summary>
    public class GreaterThanOrEqualToAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must be greater or equal to a value of specified related property.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        public GreaterThanOrEqualToAttribute(string relatedProperty) : base(Operator.GreaterThanOrEqualTo, relatedProperty) { }
    }
}
