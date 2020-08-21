
namespace Structr.AspNetCore.Validation
{
    public class NotEqualToAttribute : IsAttribute
    {
        public NotEqualToAttribute(string dependentProperty) : base(Operator.NotEqualTo, dependentProperty) { }
    }
}
