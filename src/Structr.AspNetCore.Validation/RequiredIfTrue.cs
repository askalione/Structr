
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfTrueAttribute : RequiredIfAttribute
    {
        public RequiredIfTrueAttribute(string relatedProperty) : base(relatedProperty, Operator.EqualTo, true) { }
    }
}
