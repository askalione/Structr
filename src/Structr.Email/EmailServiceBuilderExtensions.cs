using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Clients;
using Structr.Email.Clients.Smtp;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailServiceBuilderExtensions
    {
        public static EmailServiceBuilder AddFileClient(this EmailServiceBuilder builder, string path)
        {
            builder.Services.TryAddSingleton<IEmailClient>(_ => new FileEmailClient(path));

            return builder;
        }

        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, string host, int port = 25)
            => AddSmtpClient(builder, _ => new SmtpOptions(host, port));

        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, SmtpOptions options)
            => AddSmtpClient(builder, _ => options);

        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, Func<IServiceProvider, SmtpOptions> optionsFactory)
        {
            if (optionsFactory == null)
            {
                throw new ArgumentNullException(nameof(optionsFactory));
            }

            builder.Services.TryAddSingleton(serviceProvider => optionsFactory.Invoke(serviceProvider));
            builder.Services.TryAddSingleton<IEmailClient, SmtpEmailClient>();

            return builder;
        }
    }
}
