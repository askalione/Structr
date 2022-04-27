using System;

namespace Structr.Email
{
    public class EmailOptions
    {
        internal EmailAddress From { get; }
        public string? TemplateRootPath { get; set; }

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
