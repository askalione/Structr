using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods for configuring Email services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <inheritdoc cref="AddEmail(IServiceCollection, EmailAddress, Action{IServiceProvider, EmailOptions}?)"/>
        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
           string from,
           Action<EmailOptions>? configure = null)
           => AddEmail(services, new EmailAddress(from), configure);

        /// <inheritdoc cref="AddEmail(IServiceCollection, EmailAddress, Action{IServiceProvider, EmailOptions}?)"/>
        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            string from,
            Action<IServiceProvider, EmailOptions> configure)
            => AddEmail(services, new EmailAddress(from), configure);

        /// <inheritdoc cref="AddEmail(IServiceCollection, EmailAddress, Action{IServiceProvider, EmailOptions}?)"/>
        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            EmailAddress from,
            Action<EmailOptions>? configure = null)
            => AddEmail(services, _ => from, (_, options) => configure?.Invoke(options));

        /// <param name="from">Param to configure the "from" <see cref="EmailAddress"/>.</param>
        /// <inheritdoc cref="AddEmail(IServiceCollection, Func{IServiceProvider, EmailAddress}, Action{IServiceProvider, EmailOptions}?)"/>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">Delegate to configure the <see cref="EmailOptions"/>.</param>
        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            EmailAddress from,
            Action<IServiceProvider, EmailOptions> configure)
            => AddEmail(services, _ => from, configure);

        /// <summary>
        /// Add Email services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="fromFactory">Delegate to configure the "from" <see cref="EmailAddress"/>.</param>
        /// <param name="configure">Delegate to configure the <see cref="EmailOptions"/>.</param>
        /// <returns>The <see cref="EmailServiceBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="services"/> is <see langword="null"/>.</exception>
        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            Func<IServiceProvider, EmailAddress> fromFactory,
            Action<IServiceProvider, EmailOptions>? configure = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = new EmailServiceBuilder(services);

            services.TryAddSingleton(serviceProvider =>
            {
                var options = new EmailOptions(fromFactory(serviceProvider));
                configure?.Invoke(serviceProvider, options);
                return options;
            });
            services.TryAddSingleton<IEmailSender, EmailSender>();

            return builder;
        }
    }
}
