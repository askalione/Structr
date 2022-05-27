
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Marks property as required when value of related property is NOT equal to specified value.
    /// </summary>
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Marks property as required when value of related property is NOT equal to <paramref name="relatedPropertyExpectedValue"/>.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        /// <param name="relatedPropertyExpectedValue">Expected related property's value to compare with.</param>
        public RequiredIfNotAttribute(string relatedProperty, object relatedPropertyExpectedValue) : base(relatedProperty, Operator.NotEqualTo, relatedPropertyExpectedValue) { }
    }
}
