using Structr.Samples.Validation.Models;
using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Validation.Validators
{
    public class BazValidator : FooValidator<Baz>
    {
        protected override IValidationResult Validate(Baz instance)
        {
            List<IValidationFailure> failures = new List<IValidationFailure>();

            failures.AddRange(base.Validate(instance));

            if (instance.Shape == EShape.Triangle)
                failures.Add(new ValidationFailure(nameof(instance.Shape), instance.Shape, "Triangle is a bullshit. Circle is a king!"));

            return new ValidationResult(failures);
        }
    }
}
