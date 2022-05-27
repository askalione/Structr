
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfTrueAttribute : RequiredIfAttribute
    {
        /// <inheritdoc cref="RequiredIfAttribute.RequiredIfAttribute"/>
        public RequiredIfTrueAttribute(string relatedProperty) : base(relatedProperty, Operator.EqualTo, true) { }
    }
}
