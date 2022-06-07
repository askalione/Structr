
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must NOT be equal to a value of specified related property.
    /// </summary>
    /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
    public class NotEqualToAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must NOT be equal to a value of specified related property.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
        public NotEqualToAttribute(string relatedProperty) : base(Operator.NotEqualTo, relatedProperty) { }
    }
}
