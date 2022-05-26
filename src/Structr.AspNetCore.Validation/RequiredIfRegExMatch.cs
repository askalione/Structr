
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfRegExMatchAttribute(string relatedProperty, string pattern) : base(relatedProperty, Operator.RegExMatch, pattern) { }
    }
}
