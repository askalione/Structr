using Structr.Email;
using Structr.Email.Clients.Smtp;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email.Clients.Smtp
{
    public class SmtpEmailClientTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var smtpClientFactory = new FakeSmtpClientFactory(new SmtpOptions("127.0.0.1"));

            // Act
            var result = new SmtpEmailClient(smtpClientFactory);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_options_is_null(FakeSmtpClientFactory smtpClientFactory)
        {
            // Act
            Action act = () => new SmtpEmailClient(smtpClientFactory);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task SendAsync()
        {
            // Arrange
            var emailClient = new SmtpEmailClient(new FakeSmtpClientFactory(new SmtpOptions("127.0.0.1")));
            var emailData = new CustomEmailData(new List<EmailAddress>() { new EmailAddress("address@example.com") });
            emailData.From = new EmailAddress("from@example.com");

            var tempDirPath = TestDataPath.Combine("FakeSmtpClientTemp");
            Directory.CreateDirectory(tempDirPath);

            // Act
            var result = await emailClient.SendAsync(emailData, "Hello!");

            // Assert
            result.Should().BeTrue();
            IEnumerable<string> files = Directory.EnumerateFiles(tempDirPath);
            files.Should().ContainSingle();
            File.Delete(files.Single());
            Directory.Delete(tempDirPath);
        }
    }
}
