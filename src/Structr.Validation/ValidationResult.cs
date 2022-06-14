using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Validation
{
    /// <summary>
    /// Represents failures that occur during validation execution.
    /// </summary>
    public class ValidationResult : IEnumerable<ValidationFailure>
    {
        /// <summary>
        /// Returns <see langword="true"/> if the validation result has no failures.
        /// </summary>
        public bool IsValid => _failures.Count == 0;

        private readonly List<ValidationFailure> _failures;

        /// <inheritdoc cref="ValidationResult(IEnumerable{ValidationFailure})"/>
        public ValidationResult()
        {
            _failures = new List<ValidationFailure>();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="failures">The list of <see cref="ValidationFailure"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="failures"/> is <see langword="null"/>.</exception>
        public ValidationResult(IEnumerable<ValidationFailure> failures)
        {
            if (failures == null)
            {
                throw new ArgumentNullException(nameof(failures));
            }

            _failures = failures.Where(x => x != null).ToList();
        }

        public IEnumerator<ValidationFailure> GetEnumerator()
        {
            return _failures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Returns a string that contains list of <see cref="ValidationFailure"/> separated by <paramref name="separator"/>.
        /// </summary>
        /// <param name="separator">The string to use as a separator.</param>
        public string ToString(string separator)
        {
            return string.Join(separator, _failures.Select(x => x.Message));
        }

        public override string ToString()
        {
            return ToString(Environment.NewLine);
        }
    }
}
