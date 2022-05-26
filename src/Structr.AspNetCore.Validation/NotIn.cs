
namespace Structr.AspNetCore.Validation
{
    public class NotInAttribute : IsAttribute
    {
        public NotInAttribute(string relatedProperty) : base(Operator.NotIn, relatedProperty) { }
    }
}
