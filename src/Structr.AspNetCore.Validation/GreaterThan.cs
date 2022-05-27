
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be equal to a value of specified related property.
    /// </summary>
    public class GreaterThanAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public GreaterThanAttribute(string relatedProperty) : base(Operator.GreaterThan, relatedProperty) { }
    }
}
