
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be greater or equal to a value of specified related property.
    /// </summary>
    public class GreaterThanOrEqualToAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public GreaterThanOrEqualToAttribute(string relatedProperty) : base(Operator.GreaterThanOrEqualTo, relatedProperty) { }
    }
}
