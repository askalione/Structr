using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Clients;
using Structr.Email.TemplateRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
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
