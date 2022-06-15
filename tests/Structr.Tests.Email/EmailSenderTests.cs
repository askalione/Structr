using Structr.Email;
using Structr.Email.Clients;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email
{
    public class EmailSenderTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("address@example.com"));
            var emailClient = new FileEmailClient(TestDataPath.Combine("EmailSenderTemp"));

            // Act
            var result = new EmailSender(options, emailClient);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_if_options_is_null(EmailOptions options)
        {
            // Arrange
            var fileEmailClient = new FileEmailClient(TestDataPath.Combine("EmailSenderTemp"));

            // Act
            Action act = () => new EmailSender(options, fileEmailClient);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_if_emailClient_is_null(IEmailClient emailClient)
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("address@example.com"));

            // Act
            Action act = () => new EmailSender(options, emailClient);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task SendEmailAsync()
        {
            // Arrange
            var tempDirPath = TestDataPath.Combine("EmailSenderTemp");
            var options = new EmailOptions(new EmailAddress("address@example.com"));
            var emailClient = new FileEmailClient(tempDirPath);
            var emailSender = new EmailSender(options, emailClient);
            var emailMessage = new EmailMessage("member@example.com", "Hello!");

            // Act
            var result = await emailSender.SendEmailAsync(emailMessage, default(CancellationToken));

            // Assert
            result.Should().BeTrue();
            IEnumerable<string> files = Directory.EnumerateFiles(tempDirPath);
            files.Should().ContainSingle();
            File.Delete(files.Single());
            Directory.Delete(tempDirPath);
        }
    }
}
