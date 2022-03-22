using System.Linq;

namespace Structr.Validation.Internal
{
    internal class EmptyValidator<T> : Validator<T>
    {
        protected override ValidationResult Validate(T instance)
        {
            return new ValidationResult(Enumerable.Empty<ValidationFailure>());
        }
    }
}
