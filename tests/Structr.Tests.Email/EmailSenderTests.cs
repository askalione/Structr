using Structr.Email;
using Structr.Email.Clients;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email
{
    public class EmailSenderTests : IDisposable
    {
        private readonly string _tempDirPath;
        private readonly IEmailClient _emailClient;
        private readonly string _from;
        private readonly string _to;
        private readonly string _subject;
        private readonly string _message;
        private readonly string _template;
        private readonly string _templatePath;

        public EmailSenderTests()
        {
            _tempDirPath = TestDataPath.Combine("EmailSenderTemp");
            _emailClient = new FileEmailClient(_tempDirPath);

            _from = "tatyana@larina.name";
            _to = "eugene@onegin.name";
            _subject = "Letter of Tatyana to Onegin.";
            _message = string.Join(" ", "Letter of Tatyana to Onegin.", "I write this to you - what more can be said?");
            _template = string.Join(" ", "Letter of {{From}} to {{To}}.", "I write this to you - what more can be said?");

            _templatePath = TestDataPath.Combine("Letter of Tatyana to Onegin Template.txt");
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress(_from));

            // Act
            Action act = () => new EmailSender(options, _emailClient);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_if_options_is_null()
        {
            // Act
            Action act = () => new EmailSender(null!, _emailClient);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_if_emailClient_is_null()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress(_from));

            // Act
            Action act = () => new EmailSender(options, null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task SendEmailAsync_with_emailMessage()
        {
            // Arrange
            var emailMessage = new EmailMessage(_to, _message);
            emailMessage.Subject = _subject;

            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            await emailSender.SendEmailAsync(emailMessage, default(CancellationToken));

            // Assert
            GetResultFromFile().Should()
                .StartWith(string.Join(Environment.NewLine, $"From: {_from}", $"To: {_to}", $"Subject: {_subject}", "", _message));
        }

        [Fact]
        public async Task SendEmailAsync_with_emailMessage_throws_when_email_is_null()
        {
            // Arrange
            EmailMessage emailMessage = null!;
            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            Func<Task> act = () => emailSender.SendEmailAsync(emailMessage, default(CancellationToken));

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }

        [Fact]
        public async Task SendEmailAsync_with_emailTemplateMessage()
        {
            // Arrange
            var emailTemplateMessage = new EmailTemplateMessage(_to, _template, new CustomModel());
            emailTemplateMessage.Subject = _subject;

            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            await emailSender.SendEmailAsync(emailTemplateMessage, default(CancellationToken));

            // Assert
            GetResultFromFile().Should()
                .StartWith(string.Join(Environment.NewLine, $"From: {_from}", $"To: {_to}", $"Subject: {_subject}", "", _message));
        }

        [Fact]
        public async Task SendEmailAsync_with_emailTemplateMessage_throws_when_email_is_null()
        {
            // Arrange
            EmailTemplateMessage emailTemplateMessage = null!;
            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            Func<Task> act = () => emailSender.SendEmailAsync(emailTemplateMessage, default(CancellationToken));

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }

        [Fact]
        public async Task SendEmailAsync_generic_with_emailTemplateMessage()
        {
            // Arrange
            var emailTemplateMessage = new CustomEmailTemplateMessage(_to, new CustomModel());
            emailTemplateMessage.Subject = _subject;

            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            await emailSender.SendEmailAsync(emailTemplateMessage, default(CancellationToken));

            // Assert
            GetResultFromFile().Should()
                .Be(string.Join(Environment.NewLine, $"From: {_from}", $"To: {_to}", $"Subject: {_subject}", "", "Letter of Tatyana to Onegin."));
        }

        [Fact]
        public async Task SendEmailAsync_generic_with_emailTemplateMessage_throws_when_email_is_null()
        {
            // Arrange
            CustomEmailTemplateMessage emailTemplateMessage = null!;
            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            Func<Task> act = () => emailSender.SendEmailAsync(emailTemplateMessage, default(CancellationToken));

            // Assert
            await act.Should().ThrowExactlyAsync<ArgumentException>();
        }

        [Fact]
        public async Task SendEmailAsync_with_emailTemplateFileMessage()
        {
            // Arrange
            var emailTemplateFileMessage = new EmailTemplateFileMessage(_to, _templatePath, new CustomModel());
            emailTemplateFileMessage.Subject = _subject;

            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(_from)), _emailClient);

            // Act
            await emailSender.SendEmailAsync(emailTemplateFileMessage, default(CancellationToken));

            // Assert            
            GetResultFromFile().Should()
                .StartWith(string.Join(Environment.NewLine, $"From: {_from}", $"To: {_to}", $"Subject: {_subject}", "", _message));
        }

        private string GetResultFromFile()
        {
            return File.ReadAllText(Directory.EnumerateFiles(_tempDirPath).Single());
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDirPath))
            {
                foreach (var file in Directory.EnumerateFiles(_tempDirPath))
                {
                    File.Delete(file);
                }
                Directory.Delete(_tempDirPath);
            }
        }
    }
}
