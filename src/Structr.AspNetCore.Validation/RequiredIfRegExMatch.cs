
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfRegExMatchAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Marks property as required when condition for specified <paramref name="relatedProperty"/> is met.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        /// <param name="relatedPropertyPattern">Pattern with which value of related property should match.</param>
        public RequiredIfRegExMatchAttribute(string relatedProperty, string relatedPropertyPattern) : base(relatedProperty, Operator.RegExMatch, relatedPropertyPattern) { }
    }
}
