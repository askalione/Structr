using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.TemplateRenderers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static EmailServiceBuilder AddEmail(this IServiceCollection services, string from)
            => AddEmail(services, _ => new EmailAddress(from));

        public static EmailServiceBuilder AddEmail(this IServiceCollection services, EmailAddress from)
            => AddEmail(services, _ => from);

        public static EmailServiceBuilder AddEmail(this IServiceCollection services, Func<IServiceProvider, EmailAddress> fromFactory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var builder = new EmailServiceBuilder(services);

            services.TryAddSingleton(serviceProvider => new EmailOptions(fromFactory(serviceProvider)));
            services.TryAddSingleton<IEmailTemplateRenderer>(_ => new ReplaceEmailTemplateRenderer());
            services.TryAddSingleton<IEmailSender, EmailSender>();

            return builder;
        }
    }
}
