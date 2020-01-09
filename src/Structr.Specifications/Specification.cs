using System;
using System.Linq.Expressions;

namespace Structr.Specifications
{
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public Func<T, bool> ToFunc() => ToExpression().Compile();

        public bool IsSatisfiedBy(T candidate)
        {
            return ToFunc()(candidate);
        }
    }
}
