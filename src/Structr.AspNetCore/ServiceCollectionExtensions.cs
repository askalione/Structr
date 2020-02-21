using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.AspNetCore.JavaScript;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJavaScriptAlerts(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.TryAddTransient<IJavaScriptAlertProvider, JavaScriptAlertProvider>();

            return services;
        }

        public static IServiceCollection AddJavaScriptOptions(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.TryAddTransient<IJavaScriptOptionProvider, JavaScriptOptionProvider>();

            return services;
        }
    }
}
