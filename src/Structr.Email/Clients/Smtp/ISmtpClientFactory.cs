using System.Net.Mail;

namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Provides functionality for creating a SmtpClient.
    /// </summary>
    public interface ISmtpClientFactory
    {
        /// <summary>
        /// Create a <see cref="SmtpClient"/>.
        /// </summary>
        SmtpClient CreateSmtpClient();
    }
}
