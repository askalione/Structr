using System;
using System.Collections.Generic;
using System.Linq;

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
        /// List of email addresses of a recipients.
        /// </summary>
        public IEnumerable<EmailAddress> To { get; }

        /// <summary>
        /// Subject of the e-mail message.
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// Indicates whether the body of the e-mail message is in HTML.
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// List of attachments used to store data attached to the e-mail message.
        /// </summary>
        public IEnumerable<EmailAttachment>? Attachments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailData"/> class.
        /// </summary>
        /// <param name="to">The list of the <see cref="EmailAddress"/> of a recipients.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="to"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="to"/> is empty.</exception>
        public EmailData(IEnumerable<EmailAddress> to)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }
            if (to.Any() == false)
            {
                throw new ArgumentException($"Email \"To\" should have one email address at least", nameof(to));
            }

            To = to.ToList();
        }
    }
}
