
namespace Structr.AspNetCore.Validation
{
    public class GreaterThanAttribute : IsAttribute
    {
        public GreaterThanAttribute(string relatedProperty) : base(Operator.GreaterThan, relatedProperty) { }
    }
}
