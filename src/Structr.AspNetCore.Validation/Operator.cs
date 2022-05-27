
namespace Structr.AspNetCore.Validation
{
    /// <summary>
    /// Types of checking operators to be applied.
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// Checks equalty. Two null values are considered as equal.
        /// </summary>
        EqualTo,

        /// <summary>
        /// Checks that one value is equal to another. Two null values are considered as not equal.
        /// </summary>
        NotEqualTo,

        /// <summary>
        /// Checks that one value is greater then another.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Checks that one value is less then another.
        /// </summary>
        LessThan,

        /// <summary>
        /// Checks that one value is greater or equal to another.
        /// </summary>
        GreaterThanOrEqualTo,

        /// <summary>
        /// Checks that one value is less or equal to another.
        /// </summary>
        LessThanOrEqualTo,

        /// <summary>
        /// Checks that value matches a regular expression.
        /// </summary>
        RegExMatch,

        /// <summary>
        /// Checks that value doesn't match a regular expression.
        /// </summary>
        NotRegExMatch,

        /// <summary>
        /// Checks that value is included in another value.
        /// </summary>
        In,

        /// <summary>
        /// Checks that value is not included in another value.
        /// </summary>
        NotIn
    }
}