
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfNotAttribute : RequiredIfAttribute
    {
        public RequiredIfNotAttribute(string relatedProperty, object relatedValue) : base(relatedProperty, Operator.NotEqualTo, relatedValue) { }
    }
}
