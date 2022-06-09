using System;

namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Defines a set of options used for <see cref="SmtpEmailClient"/>.
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// Name or IP address of the host used for SMTP transactions.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Port used for SMTP transactions. The default value is 25.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// User name used to authenticate a sender.
        /// </summary>
        public string? User { get; set; }

        /// <summary>
        /// User password used to authenticate a sender.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Specify whether the <see cref="SmtpEmailClient"/> uses SSL to encrypt a connection.
        /// </summary>
        public bool IsSslEnabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpOptions"/> class.
        /// </summary>
        /// <param name="host">The name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">The port used for SMTP transactions. The default value is 25.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="host"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="port"/> is less than zero.</exception>
        public SmtpOptions(string host, int port = 25)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (port < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port), port, $"Port cannot be less than zero.");
            }

            Host = host;
            Port = port;
        }
    }
}
