
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Specifies that a data field value must be equal to a value of specified related property.
    /// </summary>
    public class EqualToAttribute : IsAttribute
    {
        /// <inheritdoc cref="IsAttribute.IsAttribute"/>
        public EqualToAttribute(string relatedProperty) : base(Operator.EqualTo, relatedProperty) { }
    }
}
