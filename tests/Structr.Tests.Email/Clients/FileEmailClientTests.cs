using Structr.Email;
using Structr.Email.Clients;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email.Clients
{
    public class FileEmailClientTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new FileEmailClient(TestDataPath.ContentRootPath);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_path_is_null_or_empty(string path)
        {
            // Act
            Action act = () => new FileEmailClient(path);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public async Task SendAsync()
        {
            // Arrange
            var tempDirPath = TestDataPath.Combine("FileEmailClientTemp");
            var fileEmailClient = new FileEmailClient(tempDirPath);
            var emailData = new CustomEmailData(new List<EmailAddress> { new EmailAddress("address@example.com") });

            // Act
            var result = await fileEmailClient.SendAsync(emailData, "Some message", default(CancellationToken));

            // Assert
            result.Should().BeTrue();
            IEnumerable<string> files = Directory.EnumerateFiles(tempDirPath);
            files.Should().ContainSingle();
            File.Delete(files.Single());
            Directory.Delete(tempDirPath);
        }
    }
}
