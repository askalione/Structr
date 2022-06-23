using System;

namespace Structr.Email
{
    /// <summary>
    /// Defines a set of options used for email services.
    /// </summary>
    public class EmailOptions
    {
        /// <summary>
        /// Email address of a sender.
        /// </summary>
        public EmailAddress From { get; }

        /// <summary>
        /// Root directory path with email templates.
        /// </summary>
        public string? TemplateRootPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailOptions"/> class.
        /// </summary>
        /// <param name="from">The <see cref="EmailAddress"/> of a sender.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="from"/> is <see langword="null"/>.</exception>
        public EmailOptions(EmailAddress from)
        {
            if (from == null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            From = from;
        }
    }
}
