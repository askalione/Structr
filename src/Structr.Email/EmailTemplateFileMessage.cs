using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Email
{
    /// <summary>
    /// Represents an object containing data about an email generated from template file with some model.
    /// </summary>
    public class EmailTemplateFileMessage : EmailData
    {
        /// <summary>
        /// Absolute path to a template.
        /// </summary>
        public string TemplatePath { get; }

        /// <summary>
        /// Model to render into a template.
        /// </summary>
        public object Model { get; }

        /// <param name="to">The email address of a recipient.</param>
        /// <inheritdoc cref="EmailTemplateFileMessage(IEnumerable{string},string,object)"/>
        public EmailTemplateFileMessage(string to,
            string templatePath,
            object model)
            : this(new[] { to },
                 templatePath,
                 model)
        {
        }

        /// <param name="to">The list of the emails of a recipients.</param>
        /// <inheritdoc cref="EmailTemplateFileMessage(IEnumerable{EmailAddress},string,object)"/>
        public EmailTemplateFileMessage(IEnumerable<string> to,
            string templatePath,
            object model)
            : this(to.Select(x => new EmailAddress(x)),
                 templatePath,
                 model)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateFileMessage"/> class.
        /// </summary>
        /// <param name="to">The list of the <see cref="EmailAddress"/> of a recipients.</param>
        /// <param name="templatePath">The absolute path to the template.</param>
        /// <param name="model">The model to render into the template.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="templatePath"/> is <see langword="null"/> or empty.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="model"/> is <see langword="null"/>.</exception>
        public EmailTemplateFileMessage(IEnumerable<EmailAddress> to,
            string templatePath,
            object model)
            : base(to)
        {
            if (string.IsNullOrWhiteSpace(templatePath))
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

    /// <summary>
    /// Represents an object containing data about an email generated from template file with <see cref="TModel"/>.
    /// </summary>
    /// <typeparam name="TModel">Model type.</typeparam>
    public abstract class EmailTemplateFileMessage<TModel> : EmailData
    {
        /// <summary>
        /// Absolute path to a template.
        /// </summary>
        public abstract string TemplatePath { get; }

        /// <summary>
        /// Model to render into a template.
        /// </summary>
        public TModel Model { get; }

        /// <param name="to">The email address of a recipient.</param>
        /// <inheritdoc cref="EmailTemplateFileMessage{TModel}(IEnumerable{string},TModel)"/>
        public EmailTemplateFileMessage(string to, TModel model)
            : this(new[] { to }, model)
        {
        }

        /// <param name="to">The list of the emails of a recipients.</param>
        /// <inheritdoc cref="EmailTemplateFileMessage{TModel}(IEnumerable{EmailAddress},TModel)"/>
        public EmailTemplateFileMessage(IEnumerable<string> to, TModel model)
            : this(to.Select(x => new EmailAddress(x)), model)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTemplateFileMessage{TModel}"/> class.
        /// </summary>
        /// <param name="to">The list of the <see cref="EmailAddress"/> of a recipients.</param>
        /// <param name="model">The model to render into the template.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="model"/> is <see langword="null"/>.</exception>
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
