using System;
using System.Linq.Expressions;

namespace Structr.Specifications
{
    /// <summary>
    /// Specification to which none of objects of <typeparamref name="T"/> will match.
    /// </summary>
    /// <inheritdoc/>
    public class NoneSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> ToExpression() => x => false;

        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetType() == other.GetType();
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }
    }
}
