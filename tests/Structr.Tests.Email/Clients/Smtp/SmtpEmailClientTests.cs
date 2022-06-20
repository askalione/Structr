using Structr.Email;
using Structr.Email.Clients.Smtp;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email.Clients.Smtp
{
    public class SmtpEmailClientTests : IDisposable
    {
        private string _tempDirPath;

        public SmtpEmailClientTests()
        {
            _tempDirPath = TestDataPath.Combine("FakeSmtpClientTemp");
            Directory.CreateDirectory(_tempDirPath);
        }

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
            var emailData = new CustomEmailData(new List<EmailAddress>() { new EmailAddress("eugene@onegin.name") });
            emailData.From = new EmailAddress("tatyana@larina.name");

            // Act
            var result = await emailClient.SendAsync(emailData, "I write this to you - what more can be said?");

            // Assert
            result.Should().BeTrue();
            Directory.EnumerateFiles(_tempDirPath).Should().ContainSingle();
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
