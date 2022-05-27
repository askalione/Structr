
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        public RequiredIfNotAttribute(string relatedProperty, object relatedPropertyExpectedValue) : base(relatedProperty, Operator.NotEqualTo, relatedPropertyExpectedValue) { }
    }
}
