using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Email
{
    public class EmailTemplateFileMessage : EmailData
    {
        public string TemplatePath { get; }
        public object Model { get; }

        public EmailTemplateFileMessage(string to,
            string templatePath,
            object model)
            : this(new[] { to },
                 templatePath,
                 model)
        {
        }

        public EmailTemplateFileMessage(IEnumerable<string> to,
            string templatePath,
            object model)
            : this(to.Select(x => new EmailAddress(x)),
                 templatePath,
                 model)
        {

        }

        public EmailTemplateFileMessage(IEnumerable<EmailAddress> to,
            string templatePath,
            object model)
            : base(to)
        {
            if (string.IsNullOrEmpty(templatePath))
            {
                throw new ArgumentNullException(nameof(templatePath));
            }
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            TemplatePath = templatePath;
            Model = model;
        }
    }

    public abstract class EmailTemplateFileMessage<TModel> : EmailData
    {
        public abstract string TemplatePath { get; }
        public TModel Model { get; }

        public EmailTemplateFileMessage(string to, TModel model)
            : this(new[] { to }, model)
        {
        }

        public EmailTemplateFileMessage(IEnumerable<string> to, TModel model)
            : this(to.Select(x => new EmailAddress(x)), model)
        {
        }

        public EmailTemplateFileMessage(IEnumerable<EmailAddress> to, TModel model)
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
