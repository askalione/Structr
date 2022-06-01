
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Marks property as required when value of related property is <see langword="false"/>.
    /// </summary>
    public class RequiredIfFalseAttribute : RequiredIfAttribute
    {
        /// <summary>
        /// Marks property as required when value of related property is <see langword="false"/>.
        /// </summary>
        /// <param name="relatedProperty">Related property which value should met specified conditions.</param>
        public RequiredIfFalseAttribute(string relatedProperty) : base(relatedProperty, Operator.EqualTo, false) { }
    }
}
