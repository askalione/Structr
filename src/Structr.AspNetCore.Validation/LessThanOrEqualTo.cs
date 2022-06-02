
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be less or equal to a value of specified related property.
    /// </summary>
    public class LessThanOrEqualToAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must be less or equal to a value of specified related property.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        public LessThanOrEqualToAttribute(string relatedProperty) : base(Operator.LessThanOrEqualTo, relatedProperty) { }
    }
}
