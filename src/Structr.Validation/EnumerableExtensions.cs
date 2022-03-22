using System;
using System.Collections.Generic;

namespace Structr.Validation
{
    public static class EnumerableExtensions
    {
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
