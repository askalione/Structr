
namespace Structr.AspNetCore.Validation
{
    public class InAttribute : IsAttribute
    {
        public InAttribute(string dependentProperty) : base(Operator.In, dependentProperty) { }
    }
}
