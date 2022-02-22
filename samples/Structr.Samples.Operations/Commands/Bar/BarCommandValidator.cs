using Structr.Validation;
using System.Collections.Generic;

namespace Structr.Samples.Operations.Commands.Bar
{
    public class BarCommandValidator : Validator<BarCommand>
    {
        protected override IValidationResult Validate(BarCommand command)
        {
            var failures = new List<IValidationFailure>();

            if (string.IsNullOrEmpty(command.Name))
            {
                failures.Add(new ValidationFailure(nameof(command.Name), command.Name, "Name is required"));
            }
            if (command.Name != null && command.Name.Length > 1)
            {
                failures.Add(new ValidationFailure(nameof(command.Name), command.Name.Length, "Name is too long"));
            }

            return new ValidationResult(failures);
        }
    }
}
