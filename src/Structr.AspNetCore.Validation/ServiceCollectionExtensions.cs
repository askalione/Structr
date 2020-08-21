using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using Structr.AspNetCore.Validation.Internal;
using System;

namespace Structr.AspNetCore.Validation
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
