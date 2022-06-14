using System;
using System.Linq.Expressions;

namespace Structr.Specifications.Internal
{
    internal class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _spec;

        public NotSpecification(Specification<T> spec)
        {
            if (spec == null)
            {
                throw new ArgumentNullException(nameof(spec));
            }

            _spec = spec;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expr = _spec.ToExpression();
            return Expression.Lambda<Func<T, bool>>(Expression.Not(expr.Body), expr.Parameters);
        }

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
            if (GetType() != other.GetType())
            {
                return false;
            }

            var otherSpec = other as NotSpecification<T>;
            return _spec.Equals(otherSpec._spec);
        }

        public override int GetHashCode()
        {
            return _spec.GetHashCode() ^ GetType().GetHashCode();
        }
    }
}
