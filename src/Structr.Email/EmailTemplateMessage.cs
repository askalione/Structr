using System;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing data about an email generated from template with some model.
    /// </summary>
    public class EmailTemplateMessage : EmailData
    {
        /// <summary>
        /// Template content.
        /// </summary>
        public string Template { get; }

        /// <summary>
        /// Model to render into a template.
        /// </summary>
        public object Model { get; }

        /// <param name="to">The email address of a recipient.</param>
        /// <inheritdoc cref="EmailTemplateMessage(EmailAddress,string,object)"/>
        public EmailTemplateMessage(string to,
            string template,
            object model)
            : this(new EmailAddress(to),
                 template,
                 model)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateMessage"/> class.
        /// </summary>
        /// <param name="to">The <see cref="EmailAddress"/> of a recipient.</param>
        /// <param name="template">The template content.</param>
        /// <param name="model">The model to render into the template.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="template"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="model"/> is <see langword="null"/>.</exception>
        public EmailTemplateMessage(EmailAddress to,
            string template,
            object model)
            : base(to)
        {
            if (string.IsNullOrWhiteSpace(template))
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

    /// <summary>
    /// Represents an object containing data about an email generated from template with <see cref="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel">Model type.</typeparam>
    public abstract class EmailTemplateMessage<TModel> : EmailData
    {
        /// <summary>
        /// Template content.
        /// </summary>
        public abstract string Template { get; }

        /// <summary>
        /// Model to render into a template.
        /// </summary>
        public TModel Model { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateMessage{TModel}"/> class.
        /// </summary>
        /// <param name="to">The <see cref="EmailAddress"/> of a recipient.</param>
        /// <param name="model">The model to render into the template.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="model"/> is <see langword="null"/>.</exception>
        public EmailTemplateMessage(EmailAddress to, TModel model)
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