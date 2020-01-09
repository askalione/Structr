using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Validation
{
    public class ValidationResult : IValidationResult
    {
        public bool IsValid => !_failures.Any();

        private readonly IEnumerable<IValidationFailure> _failures;

        public ValidationResult(IEnumerable<IValidationFailure> failures)
        {
            if (failures == null)
                throw new ArgumentNullException(nameof(failures));

            _failures = failures;
        }

        public IEnumerator<IValidationFailure> GetEnumerator()
        {
            return _failures.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string ToString(string separator)
        {
            return string.Join(separator, _failures.Select(x => x.ErrorMessage));
        }

        public override string ToString()
        {
            return ToString(Environment.NewLine);
        }
    }
}
