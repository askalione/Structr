
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be contained in value of specified related property.
    /// In case of array-type of related property simple inclusion will be checked. If it's not an
    /// array-type the equalty operator will be used.
    /// </summary>
    public class InAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public InAttribute(string relatedProperty) : base(Operator.In, relatedProperty) { }
    }
}
