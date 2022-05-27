
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be contained in value of specified related property.
    /// In case of array-type of related property simple inclusion will be checked. If it's not an
    /// array-type the equality operator will be used.
    /// </summary>
    public class InAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must be contained in value of specified related property.
        /// In case of array-type of related property simple inclusion will be checked. If it's not an
        /// array-type the equality operator will be used.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        public InAttribute(string relatedProperty) : base(Operator.In, relatedProperty) { }
    }
}
