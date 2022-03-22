using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Validation
{
    public class ValidationResult : IEnumerable<ValidationFailure>
    {
        public bool IsValid => _failures.Count == 0;

        private readonly List<ValidationFailure> _failures;

        public ValidationResult()
        {
            _failures = new List<ValidationFailure>();
        }

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
