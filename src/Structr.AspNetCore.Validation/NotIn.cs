
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must NOT be contained in value of specified related property.
    /// In case of array-type of related property simple inclusion will be checked. If it's not an
    /// array-type the equality operator will be used.
    /// </summary>
    /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
    public class NotInAttribute : IsAttribute
    {
        /// <summary>
        /// Specifies that a data field value must NOT be contained in value of specified related property.
        /// In case of array-type of related property simple inclusion will be checked. If it's not an
        /// array-type the equality operator will be used.
        /// </summary>
        /// <param name="relatedProperty">Related property with which value the value of validating property will be compared.</param>
        /// <remarks>Fails validation for two <see langword="null"/> values if <see cref="IsAttribute.PassOnNull"/> equals <see langword="true"/>.</remarks>
        public NotInAttribute(string relatedProperty) : base(Operator.NotIn, relatedProperty) { }
    }
}
