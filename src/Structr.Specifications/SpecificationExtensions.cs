using Structr.Specifications.Internal;

namespace Structr.Specifications
{
    public static class SpecificationExtensions
    {
        public static Specification<T> And<T>(this Specification<T> src, Specification<T> spec)
        {
            return new AndSpecification<T>(src, spec);
        }

        public static Specification<T> Or<T>(this Specification<T> src, Specification<T> spec)
        {
            return new OrSpecification<T>(src, spec);
        }

        public static Specification<T> Not<T>(this Specification<T> src)
        {
            return new NotSpecification<T>(src);
        }

        public static Specification<T> AndNot<T>(this Specification<T> src, Specification<T> spec)
        {
            return new AndSpecification<T>(src, Not(spec));
        }

        public static Specification<T> OrNot<T>(this Specification<T> src, Specification<T> spec)
        {
            return new OrSpecification<T>(src, Not(spec));
        }
    }
}
