using Structr.Email;
using Structr.Email.Clients;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email
{
    public class EmailSenderTests : IDisposable
    {
        private IEmailClient _emailClient;
        private string _tempDirPath;

        public EmailSenderTests()
        {
            _tempDirPath = TestDataPath.Combine("EmailSenderTemp");
            _emailClient = new FileEmailClient(_tempDirPath);
        }

        [Fact]
        public void Ctor()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("tatyana@larina.name"));

            // Act
            var result = new EmailSender(options, _emailClient);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_if_options_is_null(EmailOptions options)
        {
            // Act
            Action act = () => new EmailSender(options, _emailClient);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_if_emailClient_is_null(IEmailClient emailClient)
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("tatyana@larina.name"));

            // Act
            Action act = () => new EmailSender(options, emailClient);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task SendEmailAsync()
        {
            // Arrange
            string from = "tatyana@larina.name";
            string to = "eugene@onegin.name";
            string subject = "Tatyana's letter to Onegin.";
            string message = "I write this to you - what more can be said?";

            var emailSender = new EmailSender(new EmailOptions(new EmailAddress(from)), _emailClient);

            var emailMessage = new EmailMessage(to, message);
            emailMessage.Subject = subject;

            // Act
            bool result = await emailSender.SendEmailAsync(emailMessage, default(CancellationToken));

            // Assert
            result.Should().BeTrue();
            string filePath = Directory.EnumerateFiles(_tempDirPath).Single();
            string content = File.ReadAllText(filePath);
            content.Should().BeEquivalentTo(string.Join(Environment.NewLine, $"From: {from}", $"To: {to}", $"Subject: {subject}", "", message));
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
