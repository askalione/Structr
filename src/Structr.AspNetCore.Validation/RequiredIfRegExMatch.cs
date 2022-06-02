
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Marks property as required when value of related property matches specified regular expression.
    /// </summary>
    public class RequiredIfRegExMatchAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Marks property as required when value of related property matches <paramref name="relatedPropertyPattern"/>.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        /// <param name="relatedPropertyPattern">Regular expression pattern with which value of related property should match.</param>
        public RequiredIfRegExMatchAttribute(string relatedProperty, string relatedPropertyPattern) : base(relatedProperty, Operator.RegExMatch, relatedPropertyPattern) { }
    }
}
