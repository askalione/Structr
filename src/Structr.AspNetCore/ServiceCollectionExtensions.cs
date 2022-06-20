using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.AspNetCore.JavaScript;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring Structr.AspNetCore services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services assisting in transferring alerts from server side to client.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The <see cref="IServiceCollection"/> instance for further configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddJavaScriptAlerts(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IJavaScriptAlertProvider, JavaScriptAlertProvider>();

            return services;
        }

        /// <summary>
        /// Add services assisting in passing data represented by dictionary via <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The <see cref="IServiceCollection"/> instance for further configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddJavaScriptOptions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IJavaScriptOptionProvider, JavaScriptOptionProvider>();

            return services;
        }

        /// <summary>
        /// Add <see cref="IActionContextAccessor"/> service.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The <see cref="IServiceCollection"/> instance for further configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddActionContextAccessor(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            return services;
        }

        /// <summary>
        /// Add <see cref="Microsoft.AspNetCore.Mvc.IUrlHelper"/> service.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>The <see cref="IServiceCollection"/> instance for further configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddUrlHelper(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddScoped(x =>
            {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                var urlHelper = factory.GetUrlHelper(actionContext);

                return urlHelper;
            });

            return services;
        }
    }
}
