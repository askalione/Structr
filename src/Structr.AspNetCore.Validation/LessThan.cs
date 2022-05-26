
namespace Structr.AspNetCore.Validation
{
    public class LessThanAttribute : IsAttribute
    {
        public LessThanAttribute(string relatedProperty) : base(Operator.LessThan, relatedProperty) { }
    }
}
