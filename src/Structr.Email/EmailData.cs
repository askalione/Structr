using System;
using System.Collections.Generic;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing basic data about an email.
    /// </summary>
    public abstract class EmailData
    {
        /// <summary>
        /// Email address of a sender.
        /// </summary>
        public EmailAddress? From { get; set; }

        /// <summary>
        /// Email address of a recipient.
        /// </summary>
        public EmailAddress To { get; }

        /// <summary>
        /// Subject of the e-mail message.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Indicates whether the body of the email message is in HTML.
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// List of attachments used to store data attached to the email message.
        /// </summary>
        public IEnumerable<EmailAttachment>? Attachments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailData"/> class.
        /// </summary>
        /// <param name="to">The <see cref="EmailAddress"/> of a recipient.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="to"/> is <see langword="null"/>.</exception>
        public EmailData(EmailAddress to)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }
            To = to;
        }
    }
}
