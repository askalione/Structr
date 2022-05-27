
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Marks property as required when value of related property is <see langword="true"/>.
    /// </summary>
    public class RequiredIfTrueAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Marks property as required when value of related property is <see langword="true"/>.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        public RequiredIfTrueAttribute(string relatedProperty) : base(relatedProperty, Operator.EqualTo, true) { }
    }
}
