using Structr.Validation;

namespace Structr.Tests.Validation.TestUtils.Documents.Contracts
{
    internal class ContractValidator : DocumentValidator<Contract>
    {
        protected override ValidationResult Validate(Contract instance)
        {
            var failures = new List<ValidationFailure>();

            failures.AddRange(base.Validate(instance));

            if (instance.Title.Length > 10)
            {
                failures.Add(new ValidationFailure(nameof(instance.Title), instance.Title, "Title is too long."));
            }

            return failures.ToValidationResult();
        }
    }
}
