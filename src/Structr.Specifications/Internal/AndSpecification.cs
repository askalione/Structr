using System;
using System.Linq.Expressions;

namespace Structr.Specifications.Internal
{
    internal class AndSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _spec1;
        private readonly Specification<T> _spec2;

        public AndSpecification(Specification<T> spec1, Specification<T> spec2)
        {
            if (spec1 == null)
            {
                throw new ArgumentNullException(nameof(spec1));
            }
            if (spec2 == null)
            {
                throw new ArgumentNullException(nameof(spec2));
            }

            _spec1 = spec1;
            _spec2 = spec2;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return _spec1.ToExpression().AndAlso(_spec2.ToExpression());
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

            var otherSpec = other as AndSpecification<T>;
            return _spec1.Equals(otherSpec._spec1) && _spec2.Equals(otherSpec._spec2);
        }

        public override int GetHashCode()
        {
            return _spec1.GetHashCode() ^ _spec2.GetHashCode() ^ GetType().GetHashCode();
        }
    }
}
