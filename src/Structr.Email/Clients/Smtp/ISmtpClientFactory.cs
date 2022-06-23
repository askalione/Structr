namespace Structr.Email.Clients.Smtp
{
    /// <summary>
    /// Provides functionality for creating an <see cref="ISmtpClient"/>.
    /// </summary>
    public interface ISmtpClientFactory
    {
        /// <summary>
        /// Creates an <see cref="ISmtpClient"/>.
        /// </summary>
        ISmtpClient CreateSmtpClient();
    }
}
