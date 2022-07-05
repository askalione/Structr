using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Email
{
    public class EmailTemplateMessage : EmailData
    {
        public string Template { get; }
        public object Model { get; }

        public EmailTemplateMessage(string to,
            string template,
            object model)
            : this(new[] { to },
                 template,
                 model)
        { }

        public EmailTemplateMessage(IEnumerable<string> to,
            string template,
            object model)
            : this(to.Select(x => new EmailAddress(x)),
                 template,
                 model)
        { }

        public EmailTemplateMessage(IEnumerable<EmailAddress> to,
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

    public abstract class EmailTemplateMessage<TModel> : EmailData
    {
        public abstract string Template { get; }
        public TModel Model { get; }

        public EmailTemplateMessage(string to, TModel model)
            : this(new[] { to }, model)
        { }

        public EmailTemplateMessage(IEnumerable<string> to, TModel model)
            : this(to.Select(x => new EmailAddress(x)), model)
        { }

        public EmailTemplateMessage(IEnumerable<EmailAddress> to, TModel model)
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
