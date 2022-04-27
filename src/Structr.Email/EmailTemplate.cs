using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Email
{
    public class EmailTemplate : EmailData
    {
        public string Template { get; }
        public object Model { get; }

        public EmailTemplate(string to,
            string template,
            object model)
            : this(new[] { to },
                 template,
                 model)
        {
        }

        public EmailTemplate(IEnumerable<string> to,
            string template,
            object model)
            : this(to.Select(x => new EmailAddress(x)),
                 template,
                 model)
        {
        }

        public EmailTemplate(IEnumerable<EmailAddress> to,
            string template,
            object model)
            : base(to)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentNullException(nameof(template));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Template = template;
            Model = model;
        }
    }

    public abstract class EmailTemplate<TModel> : EmailData
    {
        public abstract string Template { get; }
        public TModel Model { get; }

        public EmailTemplate(string to, TModel model)
            : this(new[] { to }, model)
        {
        }

        public EmailTemplate(IEnumerable<string> to, TModel model)
            : this(to.Select(x => new EmailAddress(x)), model)
        {
        }

        public EmailTemplate(IEnumerable<EmailAddress> to, TModel model)
            : base(to)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Model = model;
        }
    }
}
