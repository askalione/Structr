using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Clients;
using Structr.Email.Clients.Smtp;
using Structr.Email.TemplateRenderers;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions_v1
    {
        public static IServiceCollection AddEmail<>(this IServiceCollection services,
            IEmailClient client,
            Action<EmailOptions>? configure = null)
            => AddEmail(services, _ => client, (_, options) => configure?.Invoke(options));

        public static IServiceCollection AddEmail(this IServiceCollection services,
            IEmailClient client,
            Action<IServiceProvider, EmailOptions>? configure = null)
            => AddEmail(services, _ => client, configure);

        public static IServiceCollection AddEmail(this IServiceCollection services,
            Func<IServiceProvider, IEmailClient> clientFactory,
            Action<EmailOptions>? configure = null)
            => AddEmail(services, clientFactory, (_, options) => configure?.Invoke(options));

        public static IServiceCollection AddEmail(this IServiceCollection services,
            Func<IServiceProvider, IEmailClient> clientFactory,
            Action<IServiceProvider, EmailOptions>? configure = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (clientFactory == null)
            {
                throw new ArgumentNullException(nameof(clientFactory));
            }

            services.TryAddSingleton(serviceProvider =>
            {
                var options = new EmailOptions();
                configure?.Invoke(serviceProvider, options);
                return options;
            });
            services.TryAddSingleton(serviceProvider => clientFactory(serviceProvider));
            services.TryAddSingleton<IEmailTemplateRenderer>(_ => new ReplaceEmailTemplateRenderer());
            services.TryAddSingleton<IEmailSender, EmailSender>();

            return services;
        }

        public static IServiceCollection AddSmtpEmail(this IServiceCollection services,
            Action<IServiceProvider, EmailOptions>? configure = null,
            Action<IServiceProvider, SmtpOptions>? configureSmtpOptions = null)
        {
            services.TryAddSingleton(serviceProvider =>
            {
                var options = new SmtpOptions();
                configureSmtpOptions?.Invoke(serviceProvider, options);
                return options;
            });
            services.TryAddSingleton<IEmailClient, SmtpEmailClient>();

            return AddEmail(services, serviceProvider => serviceProvider.GetRequiredService<IEmailClient>(), configure);
        }
    }
}
