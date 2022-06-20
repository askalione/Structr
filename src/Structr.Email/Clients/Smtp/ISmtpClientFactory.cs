namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Provides functionality for creating an ISmtpClient.
    /// </summary>
    public interface ISmtpClientFactory
    {
        /// <summary>
        /// Create an <see cref="ISmtpClient"/>.
        /// </summary>
        ISmtpClient CreateSmtpClient();
    }
}
