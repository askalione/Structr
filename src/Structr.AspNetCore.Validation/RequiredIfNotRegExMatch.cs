
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfNotRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfNotRegExMatchAttribute(string relatedValue, string pattern) : base(relatedValue, Operator.NotRegExMatch, pattern) { }
    }
}
