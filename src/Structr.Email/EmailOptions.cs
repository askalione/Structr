using Structr.Email.Clients;
using Structr.Email.TemplateRenderers;
using System;

namespace Structr.Email
{
    public class EmailOptions
    {
        public EmailAddress From { get; set; }

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
