using Structr.Validation.Internal;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Validation
{
    /// <inheritdoc cref="IValidationProvider"/>
    public class ValidationProvider : IValidationProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly ConcurrentDictionary<Type, InternalValidator> _cache = new ConcurrentDictionary<Type, InternalValidator>();

        /// <summary>
        /// Initializes a new instance of <see cref="ValidationProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
        public ValidationProvider(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            _serviceProvider = serviceProvider;
        }

        public Task<ValidationResult> ValidateAsync(object instance, CancellationToken cancellationToken = default)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var instanceType = instance.GetType();

            var validator = _cache.GetOrAdd(instanceType,
                type => (InternalValidator)Activator.CreateInstance(typeof(InternalValidator<>).MakeGenericType(type)));

            return validator.ValidateAsync(instance, _serviceProvider, cancellationToken);
        }

        public async Task ValidateAndThrowAsync(object instance, CancellationToken cancellationToken = default)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var failures = await ValidateAsync(instance, cancellationToken).ConfigureAwait(false);

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }
    }
}
