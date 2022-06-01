
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be less than a value of specified related property.
    /// </summary>
    /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
    public class LessThanAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must be less than a value of specified related property.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
        public LessThanAttribute(string relatedProperty) : base(Operator.LessThan, relatedProperty) { }
    }
}
