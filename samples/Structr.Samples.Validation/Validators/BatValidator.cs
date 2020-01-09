using Structr.Samples.Validation.Models;
using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Validation.Validators
{
    public class BatValidator : BarValidator<Bat>
    {
        protected override IValidationResult Validate(Bat instance)
        {
            List<IValidationFailure> failures = new List<IValidationFailure>();

            failures.AddRange(base.Validate(instance));

            if (instance.IsRough)
                failures.Add(new ValidationFailure(nameof(instance.IsRough), instance.IsRough, "So rough"));

            return new ValidationResult(failures);
        }
    }
}
