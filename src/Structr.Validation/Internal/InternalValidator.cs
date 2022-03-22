using System;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation.Internal
{
    internal abstract class InternalValidator
    {
        public abstract Task<ValidationResult> ValidateAsync(object instance,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken);
    }

    internal class InternalValidator<T> : InternalValidator
    {
        public override Task<ValidationResult> ValidateAsync(object instance,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            return ValidateAsync((T)instance, serviceProvider, cancellationToken);
        }

        private Task<ValidationResult> ValidateAsync(T instance,
            IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var validator = ValidatorProvider.GetValidator<T>(serviceProvider);

            return validator.ValidateAsync(instance, cancellationToken);
        }
    }
}
