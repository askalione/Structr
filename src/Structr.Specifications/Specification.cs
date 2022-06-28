using System;
using System.Linq.Expressions;

namespace Structr.Specifications
{
    /// <summary>
    /// Basic .NET implementation of Specification pattern serving as a base class for specifications.
    /// </summary>
    /// <typeparam name="T">Type of objects to which specification will be applied.</typeparam>
    public abstract class Specification<T>
    {
        /// <summary>
        /// Expression describing the condition.
        /// </summary>
        public abstract Expression<Func<T, bool>> ToExpression();

        /// <summary>
        /// Delegate representing the condition.
        /// </summary>
        /// <returns></returns>
        public Func<T, bool> ToFunc() => ToExpression().Compile();

        /// <summary>
        /// Determines whether provided object matches specified condition.
        /// </summary>
        /// <param name="candidate">Object to be tested.</param>
        /// <returns><see langword="true"/> if <paramref name="candidate"/> matches the specification's condition, otherwise <see langword="false"/>.</returns>
        public bool IsSatisfiedBy(T candidate)
        {
            return ToFunc()(candidate);
        }
    }
}
