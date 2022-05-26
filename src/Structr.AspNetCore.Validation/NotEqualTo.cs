
namespace Structr.AspNetCore.Validation
{
    public class NotEqualToAttribute : IsAttribute
    {
        public NotEqualToAttribute(string relatedProperty) : base(Operator.NotEqualTo, relatedProperty) { }
    }
}
