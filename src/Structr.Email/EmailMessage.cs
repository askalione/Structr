using System;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing data about an email.
    /// </summary>
    public class EmailMessage : EmailData
    {
        /// <summary>
        /// Email message.
        /// </summary>
        public string Message { get; }

        /// <param name="to">The email of a recipient.</param>
        /// <inheritdoc cref="EmailMessage(EmailAddress, string)"/>
        public EmailMessage(string to, string message)
            : this(new EmailAddress(to), message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="to">The <see cref="EmailAddress"/> of a recipient.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="message"/> is <see langword="null"/> or empty.</exception>
        public EmailMessage(EmailAddress to, string message)
            : base(to)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }
    }
}
