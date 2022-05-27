
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfRegExMatchAttribute(string relatedProperty, string relatedPropertyPattern) : base(relatedProperty, Operator.RegExMatch, relatedPropertyPattern) { }
    }
}
