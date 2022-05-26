
namespace Structr.AspNetCore.Validation
{
    public class LessThanOrEqualToAttribute : IsAttribute
    {
        public LessThanOrEqualToAttribute(string relatedProperty) : base(Operator.LessThanOrEqualTo, relatedProperty) { }
    }
}
