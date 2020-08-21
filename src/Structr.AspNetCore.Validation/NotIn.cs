
namespace Structr.AspNetCore.Validation
{
    public class NotInAttribute : IsAttribute
    {
        public NotInAttribute(string dependentProperty) : base(Operator.NotIn, dependentProperty) { }
    }
}
