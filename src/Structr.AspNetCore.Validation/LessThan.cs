
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be less than a value of specified related property.
    /// </summary>
    public class LessThanAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public LessThanAttribute(string relatedProperty) : base(Operator.LessThan, relatedProperty) { }
    }
}
