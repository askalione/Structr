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
            var smtpClientFactory = new FakeSmtpClientFactory(_tempDirPath);

            // Act
            Action act = () => new SmtpEmailClient(smtpClientFactory);

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_is_null()
        {
            // Act
            Action act = () => new SmtpEmailClient(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task SendAsync()
        {
            // Arrange
            var emailClient = new SmtpEmailClient(new FakeSmtpClientFactory(_tempDirPath));
            var emailData = new CustomEmailData(new EmailAddress("eugene@onegin.name"));
            emailData.From = new EmailAddress("tatyana@larina.name");

            // Act
            Func<Task> act = () => emailClient.SendAsync(emailData, "I write this to you - what more can be said?");

            // Assert
            await act.Should().NotThrowAsync();
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
