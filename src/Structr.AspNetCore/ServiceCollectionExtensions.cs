using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.AspNetCore.Client.Alerts;
using Structr.AspNetCore.Client.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="ServiceCollection"/> extension methods for configuring AspNetCore services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds basic ASP.NET Core services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddAspNetCore(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddHttpContextAccessor();
            services.AddActionContextAccessor();
            services.AddUrlHelper();
            services.AddClientAlerts();
            services.AddClientOptions();

            return services;
        }

        /// <summary>
        /// Add services assisting in transferring alerts from server side to client.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> instance for further configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddClientAlerts(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IClientAlertProvider, ClientAlertProvider>();

            return services;
        }

        /// <summary>
        /// Add services assisting in passing data represented by dictionary via <see cref="Microsoft.AspNetCore.Http.HttpContext.Items"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> instance for further configuration.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="services"/> is null.</exception>
        public static IServiceCollection AddClientOptions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IClientOptionProvider, ClientOptionProvider>();

            return services;
        }

        /// <summary>
        /// Add <see cref="IActionContextAccessor"/> service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
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
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
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
