
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfNotRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfNotRegExMatchAttribute(string relatedProperty, string relatedPropertyPattern) : base(relatedProperty, Operator.NotRegExMatch, relatedPropertyPattern) { }
    }
}
