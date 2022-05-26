
namespace Structr.AspNetCore.Validation
{
    public class EqualToAttribute : IsAttribute
    {
        public EqualToAttribute(string relatedProperty) : base(Operator.EqualTo, relatedProperty) { }
    }
}
