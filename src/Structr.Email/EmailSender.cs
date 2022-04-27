using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Structr.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _options;
        private readonly IEmailClient _client;
        private readonly IEmailTemplateRenderer _templateRenderer;

        public EmailSender(EmailOptions options, IEmailClient client, IEmailTemplateRenderer templateRenderer)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }
            if (templateRenderer == null)
            {
                throw new ArgumentNullException(nameof(templateRenderer));
            }

            _options = options;
            _client = client;
            _templateRenderer = templateRenderer;
        }

        public Task<bool> SendEmailAsync(EmailMessage email, CancellationToken cancellationToken = default)
            => SendEmailAsync(email, email.Message, cancellationToken);

        public Task<bool> SendEmailAsync(EmailTemplate email, CancellationToken cancellationToken = default)
            => SendEmailTemplateAsync(email, email.Template, email.Model, cancellationToken);

        public Task<bool> SendEmailAsync<TModel>(EmailTemplate<TModel> email, CancellationToken cancellationToken = default)
            => SendEmailTemplateAsync(email, email.Template, email.Model!, cancellationToken);

        public Task<bool> SendEmailAsync<TModel>(EmailTemplateFile email, CancellationToken cancellationToken = default)
            => SendEmailTemplateFileAsync(email, email.TemplatePath, email.Model!, cancellationToken);

        public Task<bool> SendEmailAsync<TModel>(EmailTemplateFile<TModel> email, CancellationToken cancellationToken = default)
            => SendEmailTemplateFileAsync(email, email.TemplatePath, email.Model!, cancellationToken);

        private Task<bool> SendEmailTemplateFileAsync(EmailData emailData, string templatePath, object model, CancellationToken cancellationToken)
        {
            var template = "";

            using (var sr = new StreamReader(File.OpenRead(templatePath)))
            {
                template = sr.ReadToEnd();
            }

            return SendEmailTemplateAsync(emailData, template, model, cancellationToken);
        }

        private async Task<bool> SendEmailTemplateAsync(EmailData emailData, string template, object model, CancellationToken cancellationToken)
        {
            var body = await _templateRenderer.RenderAsync(template, model).ConfigureAwait(false);
            return await SendEmailAsync(emailData, body, cancellationToken).ConfigureAwait(false);
        }

        private Task<bool> SendEmailAsync(EmailData emailData, string body, CancellationToken cancellationToken)
        {
            emailData.From = emailData.From ?? _options.From;
            if (string.IsNullOrEmpty(emailData.From?.Address))
            {
                throw new InvalidOperationException($"Email \"From\" not specified.");
            }

            return _client.SendAsync(emailData, body, cancellationToken);
        }
    }
}
