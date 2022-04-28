using Structr.Email;
using Structr.Samples.Email.Models;

namespace Structr.Samples.Email
{
    public class App : IApp
    {
        private readonly IEmailSender _emailSender;

        public App(IEmailSender emailSender)
        {
            if (emailSender == null)
            {
                throw new ArgumentNullException(nameof(emailSender));
            }

            _emailSender = emailSender;
        }

        public async Task RunAsync()
        {
            // Send simple email message
            await SendEmailMessageAsync();

            // Send email via template
            await SendEmailTemplateAsync();

            // Send email via Razor template file
            await SendEmailTemplateFileAsync();
        }

        private async Task SendEmailMessageAsync()
        {
            await _emailSender.SendEmailAsync(new EmailMessage("to@example.com", "Simple email message.")
            {
                Subject = "Welcome to Structr",
                Attachments = new[] {
                    new EmailAttachment(AppHelper.GetRootPath("Attachments/Attachment.txt"), "text/plain")
                }
            });
        }

#if RAZOR
        private async Task SendEmailTemplateAsync()
        {
            var template = $"Hello, world!{Environment.NewLine}@Model.Message";
            await _emailSender.SendEmailAsync(new EmailTemplate("to@example.com",
                template,
                new { Message = "Send email via template" })
            {
                Subject = "Welcome to Structr"
            });
        }

        private async Task SendEmailTemplateFileAsync()
        {
            var barEmail = new BarEmail
            {
                Message = "Email via template file."
            };
            await _emailSender.SendEmailAsync(new BarEmailTemplateFile("to@example.com", barEmail)
            {
                Subject = "Welcome to Structr"
            });
        }
#else
        private async Task SendEmailTemplateAsync()
        {
            var template = $"Hello, world!{Environment.NewLine}{{{{Message}}}}";
            await _emailSender.SendEmailAsync(new EmailTemplateMessage("to@example.com",
                template,
                new { Message = "Send email via template" })
            {
                Subject = "Welcome to Structr"
            });
        }

        private async Task SendEmailTemplateFileAsync()
        {
            var fooEmail = new FooEmailModel
            {
                Message = "Email via template file."
            };
            await _emailSender.SendEmailAsync(new FooEmailTemplateFileMessage("to@example.com", fooEmail)
            {
                Subject = "Welcome to Structr"
            });
        }
#endif
    }
}
