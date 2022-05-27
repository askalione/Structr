
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfFalseAttribute : RequiredIfAttribute
    {
        /// <inheritdoc cref="RequiredIfAttribute.RequiredIfAttribute"/>
        public RequiredIfFalseAttribute(string relatedProperty) : base(relatedProperty, Operator.EqualTo, false) { }
    }
}
