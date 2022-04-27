using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Clients.Smtp;
using Structr.Email.TemplateRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions_v3
    {
        public static EmailServiceBuilder AddEmail<TClient>(this IServiceCollection services, Func<IServiceProvider, EmailAddress> fromFactory)
            where TClient : IEmailClient
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (fromFactory == null)
            {
                throw new ArgumentNullException(nameof(fromFactory));
            }

            var builder = new EmailServiceBuilder(services);

            services.TryAddSingleton(typeof(IEmailClient), typeof(TClient));
            services.TryAddSingleton<IEmailTemplateRenderer>(_ => new ReplaceEmailTemplateRenderer());
            services.TryAddSingleton<IEmailSender>(serviceProvider =>
            {
                var from = fromFactory(serviceProvider);
                var client = serviceProvider.GetRequiredService<IEmailClient>();
                var templateRenderer = serviceProvider.GetRequiredService<IEmailTemplateRenderer>();

                return new EmailSender(from, client, templateRenderer);
            });

            return builder;
        }

        public static EmailServiceBuilder AddSmtpEmail(this IServiceCollection services,
            Func<IServiceProvider, EmailAddress> fromFactory,
            Action<IServiceProvider, SmtpOptions> configure)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var builder = new EmailServiceBuilder(services);

            services.TryAddSingleton(serviceProvider =>
            {
                var options = new SmtpOptions();
                configure.Invoke(serviceProvider, options);
                return options;
            });

            AddEmail<SmtpEmailClient>(services, fromFactory);

            return builder;
        }
    }
}
