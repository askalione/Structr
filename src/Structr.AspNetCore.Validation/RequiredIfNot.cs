
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        /// <inheritdoc cref="RequiredIfAttribute.RequiredIfAttribute"/>
        public RequiredIfNotAttribute(string relatedProperty, object relatedPropertyExpectedValue) : base(relatedProperty, Operator.NotEqualTo, relatedPropertyExpectedValue) { }
    }
}
