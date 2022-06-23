using Structr.Validation;

namespace Structr.Tests.Validation.TestUtils.Documents
{
    internal abstract class DocumentValidator<T> : Validator<T> where T : Document
    {
        protected override ValidationResult Validate(T instance)
        {
            var failures = new List<ValidationFailure>();

            if (instance.Number.Length != 5)
            {
                failures.Add(new ValidationFailure(nameof(instance.Number), instance.Number, "Document number should be 5 characters."));
            }

            return failures.ToValidationResult();
        }
    }
}
