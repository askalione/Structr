using Structr.Samples.Validation.Models;
using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Validation.Validators
{
    public abstract class BarValidator<T> : FooValidator<T> where T : Bar
    {
        protected override IValidationResult Validate(T instance)
        {
            List<IValidationFailure> failures = new List<IValidationFailure>();

            failures.AddRange(base.Validate(instance));

            if (instance.Length > 10)
                failures.Add(new ValidationFailure(nameof(instance.Length), instance.Length, "Too long"));

            return new ValidationResult(failures);
        }
    }
}
