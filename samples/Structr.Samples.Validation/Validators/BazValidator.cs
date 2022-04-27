using Structr.Samples.Validation.Models;
using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Validation.Validators
{
    public class BazValidator : FooValidator<Baz>
    {
        protected override ValidationResult Validate(Baz instance)
        {
            var failures = new List<ValidationFailure>();

            failures.AddRange(base.Validate(instance));

            if (instance.Shape == Shape.Triangle)
            {
                failures.Add(new ValidationFailure(nameof(instance.Shape), instance.Shape, "Triangle is a bullshit. Circle is a king!"));
            }

            return failures.ToValidationResult();
        }
    }
}
