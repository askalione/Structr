using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.TemplateRenderers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
           string from,
           Action<EmailOptions>? configure = null)
           => AddEmail(services, new EmailAddress(from), configure);

        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            string from,
            Action<IServiceProvider, EmailOptions> configure)
            => AddEmail(services, new EmailAddress(from), configure);

        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            EmailAddress from,
            Action<EmailOptions>? configure = null)
            => AddEmail(services, _ => from, (_, options) => configure?.Invoke(options));

        public static EmailServiceBuilder AddEmail(this IServiceCollection services,
            EmailAddress from,
            Action<IServiceProvider, EmailOptions> configure)
            => AddEmail(services, _ => from, configure);

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
