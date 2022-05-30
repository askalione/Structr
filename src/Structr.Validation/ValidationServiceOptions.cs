using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Validation
{
    /// <summary>
    /// Defines a set of options used for configure services.
    /// </summary>
    public class ValidationServiceOptions
    {
        /// <summary>
        /// Determines a type of validation provider <see cref="IValidationProvider"/>. The <see cref="ValidationProvider"/> by default.
        /// </summary>
        public Type ProviderType { get; set; }

        /// <summary>
        /// Specifies the lifetime of a validation provider in an <see cref="IServiceCollection"/>. The <see cref="ServiceLifetime.Scoped"/> by default.
        /// </summary>
        public ServiceLifetime Lifetime { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ValidationServiceOptions"/> with default values.
        /// </summary>
        public ValidationServiceOptions()
        {
            ProviderType = typeof(ValidationProvider);
            Lifetime = ServiceLifetime.Scoped;
        }
    }
}
