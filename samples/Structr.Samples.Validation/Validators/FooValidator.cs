using Structr.Samples.Validation.Models;
using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Validation.Validators
{
    public abstract class FooValidator<T> : Validator<T> where T : Foo
    {
        protected override IValidationResult Validate(T instance)
        {
            List<IValidationFailure> failures = new List<IValidationFailure>();

            if (instance.Color == EColor.Red)
                failures.Add(new ValidationFailure(nameof(instance.Color), instance.Color, "Red color is wrong color"));

            if (instance.Weight > 100)
                failures.Add(new ValidationFailure(nameof(instance.Weight), instance.Weight, "Too fat"));

            if (instance.Height > 200)
                failures.Add(new ValidationFailure(nameof(instance.Height), instance.Height, "Too tall"));

            return new ValidationResult(failures);
        }
    }
}
