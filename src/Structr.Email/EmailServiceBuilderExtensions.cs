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

        /// <inheritdoc cref="AddSmtpClient(EmailServiceBuilder, string, int, Action{SmtpOptions})"/>
        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, string host, Action<SmtpOptions>? configure = null)
            => AddSmtpClient(builder, host, 25, configure);

        /// <inheritdoc cref="AddSmtpClient(EmailServiceBuilder, string, int, Action{IServiceProvider, SmtpOptions})"/>
        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder, string host, int port, Action<SmtpOptions>? configure = null)
            => AddSmtpClient(builder, host, port, (_, options) => configure?.Invoke(options));

        /// <summary>
        /// Adds <see cref="SmtpEmailClient"/> and related services to the <see cref="EmailServiceBuilder"/>.<see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="EmailServiceBuilder"/>.</param>
        /// <param name="host">The name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">The port used for SMTP transactions. The default value is 25.</param>
        /// <param name="configure">Delegate to configure the <see cref="SmtpOptions"/>.</param>
        /// <returns>The <see cref="EmailServiceBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="host"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="port"/> is less or equal then zero.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="configure"/> is <see langword="null"/>.</exception>
        public static EmailServiceBuilder AddSmtpClient(this EmailServiceBuilder builder,
            string host,
            int port,
            Action<IServiceProvider, SmtpOptions> configure)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (port <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port), port, "Port must be greater then zero.");
            }
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.TryAddSingleton(serviceProvider =>
            {
                var options = new SmtpOptions(host, port);
                configure.Invoke(serviceProvider, options);
                return options;
            });
            builder.Services.TryAddSingleton<ISmtpClientFactory, SmtpClientFactory>();
            builder.Services.TryAddSingleton<IEmailClient, SmtpEmailClient>();

            return builder;
        }
    }
}
