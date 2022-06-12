using Structr.Specifications.Internal;

namespace Structr.Specifications
{
    /// <summary>
    /// Extensions for <see cref="Specification{T}"/> allowing building of new specifications from
    /// existing ones using logical operators.
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// Creates specification which will be satisfied only when both specifications will be satisfied by provided instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of objects to which specification will be applied.</typeparam>
        /// <param name="src">First specification to be satisfied.</param>
        /// <param name="spec">Second specification to be satisfied.</param>
        /// <returns>Specification combined from two specifications using AND logical operator.</returns>
        public static Specification<T> And<T>(this Specification<T> src, Specification<T> spec)
        {
            return new AndSpecification<T>(src, spec);
        }

        /// <summary>
        /// Creates specification which will be satisfied when at least one two specifications will be satisfied by provided instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of objects to which specification will be applied.</typeparam>
        /// <param name="src">First specification to be satisfied.</param>
        /// <param name="spec">Second specification to be satisfied.</param>
        /// <returns>Specification combined from two specifications using OR logical operator.</returns>
        public static Specification<T> Or<T>(this Specification<T> src, Specification<T> spec)
        {
            return new OrSpecification<T>(src, spec);
        }

        /// <summary>
        /// Creates specification which will be satisfied when given specification won't be satisfied by provided instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of objects to which specification will be applied.</typeparam>
        /// <param name="src">Specification that should't be satisfied.</param>
        /// <returns>Specification created from source specification using NOT logical operator.</returns>
        public static Specification<T> Not<T>(this Specification<T> src)
        {
            return new NotSpecification<T>(src);
        }

        /// <summary>
        /// Creates specification which will be satisfied when first specification will and second will not be satisfied by provided instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of objects to which specification will be applied.</typeparam>
        /// <param name="src">Secification to be satisfied.</param>
        /// <param name="spec">Specification that should't be satisfied.</param>
        /// <returns>Specification combined from two specifications using AND and NOT logical operators.</returns>
        public static Specification<T> AndNot<T>(this Specification<T> src, Specification<T> spec)
        {
            return new AndSpecification<T>(src, Not(spec));
        }

        /// <summary>
        /// Creates specification which will be satisfied when first specification will or second will not be satisfied by provided instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of objects to which specification will be applied.</typeparam>
        /// <param name="src">Specification to be satisfied.</param>
        /// <param name="spec">Specification that should't be satisfied.</param>
        /// <returns>Specification combined from two specifications using AND and NOT logical operators.</returns>
        public static Specification<T> OrNot<T>(this Specification<T> src, Specification<T> spec)
        {
            return new OrSpecification<T>(src, Not(spec));
        }
    }
}
