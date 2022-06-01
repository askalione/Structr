using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Structr.AspNetCore.Validation.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Allows to use an extended set of validation attributes in AspNetCore applications.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddAspNetCoreValidation(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IValidationAttributeAdapterProvider, InternalValidationAttributeAdapterProvider>();

            return services;
        }
    }
}
