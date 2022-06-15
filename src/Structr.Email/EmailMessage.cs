using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing data about an email.
    /// </summary>
    public class EmailMessage : EmailData
    {
        /// <summary>
        /// E-mail message.
        /// </summary>
        public string Message { get; }

        /// <param name="to">The email of a recipient.</param>
        /// <inheritdoc cref="EmailMessage(IEnumerable{string}, string)"/>
        public EmailMessage(string to, string message)
            : this(new[] { to }, message)
        {
        }

        /// <param name="to">The list of the emails of a recipients.</param>
        /// <inheritdoc cref="EmailMessage(IEnumerable{EmailAddress}, string)"/>
        public EmailMessage(IEnumerable<string> to, string message)
            : this(to.Select(x => new EmailAddress(x)), message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage"/> class.
        /// </summary>
        /// <param name="to">The list of the <see cref="EmailAddress"/> of a recipients.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="to"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="to"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="message"/> is <see langword="null"/> or empty.</exception>
        public EmailMessage(IEnumerable<EmailAddress> to, string message)
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
