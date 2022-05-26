
namespace Structr.AspNetCore.Validation
{
    public class RequiredIfFalseAttribute : RequiredIfAttribute
    {
        public RequiredIfFalseAttribute(string relatedProperty) : base(relatedProperty, Operator.EqualTo, false) { }
    }
}
