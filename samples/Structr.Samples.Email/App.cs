using Structr.Email;

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
            await _emailSender.SendEmailAsync(new EmailMessage("to@example.com", "Hello world!")
            {
                Subject = "Welcome to Structr"
            });
        }
    }
}
