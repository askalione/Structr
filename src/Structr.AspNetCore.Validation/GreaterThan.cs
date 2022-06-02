
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be greater than a value of specified related property.
    /// </summary>
    /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
    public class GreaterThanAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must be greater than a value of specified related property.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
        public GreaterThanAttribute(string relatedProperty) : base(Operator.GreaterThan, relatedProperty) { }
    }
}
