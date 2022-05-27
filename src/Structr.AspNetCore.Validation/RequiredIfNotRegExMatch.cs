
namespace Structr.AspNetCore.Validation
{
    /// <inheritdoc cref="RequiredIfAttribute"/>
    public class RequiredIfNotRegExMatchAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Marks property as required when condition for specified <paramref name="relatedProperty"/> is met.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        /// <param name="relatedPropertyPattern">Pattern with which value of related property shouldn't match.</param>
        public RequiredIfNotRegExMatchAttribute(string relatedProperty, string relatedPropertyPattern) : base(relatedProperty, Operator.NotRegExMatch, relatedPropertyPattern) { }
    }
}
