using System;
using System.Collections.Generic;

namespace Structr.Validation
{
    /// <summary>
    /// Extensions methods for <see cref="IEnumerable{}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Build a <see cref="ValidationResult"/> with specified list of <see cref="ValidationFailure"/>.
        /// </summary>
        /// <param name="failures">The list of <see cref="ValidationFailure"/>.</param>
        /// <returns>The <see cref="ValidationResult"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="failures"/> is <see langword="null"/>.</exception>
        public static ValidationResult ToValidationResult(this IEnumerable<ValidationFailure> failures)
        {
            if (failures == null)
            {
                throw new ArgumentNullException(nameof(failures));
            }

            return new ValidationResult(failures);
        }
    }
}
