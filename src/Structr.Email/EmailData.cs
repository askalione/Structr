using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Structr.Email
{
    public abstract class EmailData
    {
        public EmailAddress? From { get; set; }
        public IEnumerable<EmailAddress> To { get; }
        public string? Subject { get; set; }
        public bool IsHtml { get; set; }
        //public IEnumerable<EmailAttachment> Attachments { get; set; }

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
