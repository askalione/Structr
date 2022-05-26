
namespace Structr.AspNetCore.Validation
{
    public class GreaterThanOrEqualToAttribute : IsAttribute
    {
        public GreaterThanOrEqualToAttribute(string relatedProperty) : base(Operator.GreaterThanOrEqualTo, relatedProperty) { }
    }
}
