
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be less or equal to a value of specified related property.
    /// </summary>
    public class LessThanOrEqualToAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public LessThanOrEqualToAttribute(string relatedProperty) : base(Operator.LessThanOrEqualTo, relatedProperty) { }
    }
}
