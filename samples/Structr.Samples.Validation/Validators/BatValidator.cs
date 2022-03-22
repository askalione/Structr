using Structr.Samples.Validation.Models;
using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Validation.Validators
{
    public class BatValidator : BarValidator<Bat>
    {
        protected override ValidationResult Validate(Bat instance)
        {
            var failures = new List<ValidationFailure>();

            failures.AddRange(base.Validate(instance));

            if (instance.IsRough)
            {
                failures.Add(new ValidationFailure(nameof(instance.IsRough), instance.IsRough, "So rough"));
            }

            return failures.ToValidationResult();
        }
    }
}
