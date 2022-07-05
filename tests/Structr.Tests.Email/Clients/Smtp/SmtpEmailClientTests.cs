using Moq;
using Structr.Email;
using Structr.Email.Clients.Smtp;
using Structr.Tests.Email.TestUtils;
using System.Net.Mail;
using System.Text;

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
            // Act
            Action act = () => new SmtpEmailClient(Mock.Of<ISmtpClientFactory>());

            // Assert
            act.Should().NotThrow();
        }

        [Fact]
        public void Ctor_throws_when_options_is_null()
        {
            // Act
            Action act = () => new SmtpEmailClient(null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task SendAsync()
        {
            // Arrange
            MailMessage? result = null;

            var smtpClientMock = new Mock<ISmtpClient>();
            smtpClientMock.Setup(x => x.SendAsync(It.IsAny<MailMessage>(), It.IsAny<CancellationToken>()))
                .Callback<MailMessage, CancellationToken>((mm, ct) => result = mm);

            var smtpClientFactoryMock = new Mock<ISmtpClientFactory>();
            smtpClientFactoryMock.Setup(x => x.CreateSmtpClient()).
                Returns(smtpClientMock.Object);

            var emailClient = new SmtpEmailClient(smtpClientFactoryMock.Object);
            var emailData = new CustomEmailData(new EmailAddress("eugene@onegin.name", "Onegin"));
            emailData.From = new EmailAddress("tatyana@larina.name", "Tatyana");
            emailData.Subject = "Latter from Tatyana to Onegin";
            emailData.IsHtml = true;

            // Act
            await emailClient.SendAsync(emailData, "I write this to you - <i>what more can be said</i>?");

            // Assert
            var expected = new MailMessage
            {
                Subject = "Latter from Tatyana to Onegin",
                Body = "I write this to you - <i>what more can be said</i>?",
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,

                From = new MailAddress("tatyana@larina.name", "Tatyana"),
            };
            expected.To.Add(new MailAddress("eugene@onegin.name", "Onegin"));
            result.Should().BeEquivalentTo(expected);
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
