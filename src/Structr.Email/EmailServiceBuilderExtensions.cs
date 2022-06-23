using Microsoft.Extensions.DependencyInjection.Extensions;
using Structr.Email;
using Structr.Email.Clients;
using Structr.Email.Clients.Smtp;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="EmailServiceBuilder"/>.
    /// </summary>
    public static class EmailServiceBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="FileEmailClient"/> and related services to the <see cref="EmailServiceBuilder"/>.<see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EmailServiceBuilder"/>.</param>
        /// <param name="path">Absolute path to the email directory.</param>
        /// <returns>The <see cref="EmailServiceBuilder"/>.</returns>
        public static EmailServiceBuilder AddFileClient(this EmailServiceBuilder builder, string path)
        {
            builder.Services.TryAddSingleton<IEmailClient>(_ => new FileEmailClient(path));

            return builder;
        }

        /// <param name="host">The name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">The port used for SMTP transactions. The default value is 25.</param>
        /// <inheritdoc cref="AddSmtpClient(EmailServiceBuilder, SmtpOptions)"/>
        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, string host, int port = 25, Action<SmtpOptions>? configure = null)
            => AddSmtpClient(builder, host, port, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="SmtpEmailClient"/> and related services to the <see cref="EmailServiceBuilder"/>.<see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EmailServiceBuilder"/>.</param>
        /// <param name="optionsFactory">The delegate for configure <see cref="SmtpOptions"/>.</param>
        /// <returns>The <see cref="EmailServiceBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="optionsFactory"/> is <see langword="null"/>.</exception>
        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, string host, int port = 25, Action<IServiceProvider, SmtpOptions>? configure = null)
        {
            builder.Services.TryAddSingleton(serviceProvider =>
            {
                var options = new SmtpOptions(host, port);
                configure?.Invoke(serviceProvider, options);
                return options;
            });
            builder.Services.TryAddSingleton<ISmtpClientFactory, SmtpClientFactory>();
            builder.Services.TryAddSingleton<IEmailClient, SmtpEmailClient>();

            return builder;
        }
    }
}
