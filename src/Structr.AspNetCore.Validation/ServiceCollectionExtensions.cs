using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Structr.AspNetCore.Validation.Internal;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAspNetCoreValidation(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IValidationAttributeAdapterProvider, InternalValidationAttributeAdapterProvider>();

            return services;
        }
    }
}
