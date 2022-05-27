
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must NOT be equal to a value of specified related property.
    /// </summary>
    public class NotEqualToAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public NotEqualToAttribute(string relatedProperty) : base(Operator.NotEqualTo, relatedProperty) { }
    }
}
