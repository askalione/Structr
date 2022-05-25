
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Types of checking to be applied.
    /// </summary>
    public enum Operator
    {
        EqualTo,
        NotEqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        RegExMatch,
        NotRegExMatch,
        In,
        NotIn
    }
}