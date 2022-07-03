using Structr.Email.TemplateRenderers;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    /// <inheritdoc cref="IEmailSender"/>
    public class EmailSender : IEmailSender // ???: make internal?
    {
        private readonly EmailOptions _options;
        private readonly IEmailClient _client;
        private readonly IEmailTemplateRenderer _templateRenderer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSender"/> class.
        /// </summary>
        /// <param name="options">The <see cref="EmailOptions"/>.</param>
        /// <param name="client">The <see cref="IEmailClient"/>.</param>
        /// <param name="templateRenderer">The <see cref="IEmailTemplateRenderer"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="options"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="client"/> is <see langword="null"/>.</exception>
        public EmailSender(EmailOptions options, IEmailClient client, IEmailTemplateRenderer? templateRenderer = null)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _options = options;
            _client = client;
            _templateRenderer = templateRenderer ?? new ReplaceEmailTemplateRenderer();
        }

        public Task SendEmailAsync(EmailMessage email, CancellationToken cancellationToken = default)
            => SendEmailAsync(email, email.Message, cancellationToken);

        public Task SendEmailAsync(EmailTemplateMessage email, CancellationToken cancellationToken = default)
            => SendEmailTemplateAsync(email, email.Template, email.Model, cancellationToken);

        public Task SendEmailAsync<TModel>(EmailTemplateMessage<TModel> email, CancellationToken cancellationToken = default)
            => SendEmailTemplateAsync(email, email.Template, email.Model!, cancellationToken);

        public Task SendEmailAsync(EmailTemplateFileMessage email, CancellationToken cancellationToken = default)
            => SendEmailTemplateFileAsync(email, email.TemplatePath, email.Model!, cancellationToken);

        public Task SendEmailAsync<TModel>(EmailTemplateFileMessage<TModel> email, CancellationToken cancellationToken = default)
            => SendEmailTemplateFileAsync(email, email.TemplatePath, email.Model!, cancellationToken);

        private Task SendEmailTemplateFileAsync(EmailData emailData, string templatePath, object model, CancellationToken cancellationToken)
        {
            var template = "";

            string templateFilePath = Path.Combine(_options.TemplateRootPath ?? "", templatePath);
            using (var sr = new StreamReader(File.OpenRead(templateFilePath)))
            {
                template = sr.ReadToEnd();
            }

            return SendEmailTemplateAsync(emailData, template, model, cancellationToken);
        }

        private async Task SendEmailTemplateAsync(EmailData emailData, string template, object model, CancellationToken cancellationToken)
        {
            string body = await _templateRenderer.RenderAsync(template, model).ConfigureAwait(false);
            await SendEmailAsync(emailData, body, cancellationToken).ConfigureAwait(false);
        }

        private Task SendEmailAsync(EmailData emailData, string body, CancellationToken cancellationToken)
        {
            emailData.From = emailData.From ?? _options.From;
            if (string.IsNullOrWhiteSpace(emailData.From?.Address))
            {
                throw new InvalidOperationException($"Email \"From\" not specified.");
            }

            return _client.SendAsync(emailData, body, cancellationToken);
        }
    }
}
