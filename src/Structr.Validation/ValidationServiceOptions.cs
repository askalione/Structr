using Microsoft.Extensions.DependencyInjection;
using System;

namespace Structr.Validation
{
    /// <summary>
    /// Defines a set of options used for validation services.
    /// </summary>
    public class ValidationServiceOptions
    {
        /// <summary>
        /// Determines a type of validation provider <see cref="IValidationProvider"/>. The <see cref="ValidationProvider"/> by default.
        /// </summary>
        public Type ProviderServiceType { get; set; }

        /// <summary>
        /// Specifies the lifetime of a validation provider in an <see cref="IServiceCollection"/>. The <see cref="ServiceLifetime.Scoped"/> by default.
        /// </summary>
        public ServiceLifetime ProviderServiceLifetime { get; set; }

        /// <summary>
        /// Initializes an instance of <see cref="ValidationServiceOptions"/> with default values.
        /// </summary>
        public ValidationServiceOptions()
        {
            ProviderServiceType = typeof(ValidationProvider);
            ProviderServiceLifetime = ServiceLifetime.Transient;
        }
    }
}
