using System;
using System.Linq.Expressions;

namespace Structr.Specifications
{
    /// <summary>
    /// Specification to which all objects of <typeparamref name="T"/> will match.
    /// </summary>
    /// <inheritdoc/>
    public class AnySpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> ToExpression() => x => true;

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
