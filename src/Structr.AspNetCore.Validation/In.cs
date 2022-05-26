
namespace Structr.AspNetCore.Validation
{
    public class InAttribute : IsAttribute
    {
        public InAttribute(string relatedProperty) : base(Operator.In, relatedProperty) { }
    }
}
