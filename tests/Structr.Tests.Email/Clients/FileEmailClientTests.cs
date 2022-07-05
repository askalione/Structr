using Structr.Email;
using Structr.Email.Clients;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email.Clients
{
    public class FileEmailClientTests : IDisposable
    {
        private string _tempDirPath;

        public FileEmailClientTests()
        {
            _tempDirPath = TestDataPath.Combine("FileEmailClientTemp");
        }

        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new FileEmailClient(TestDataPath.ContentRootPath);

            // Assert
            act.Should().NotThrow();
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
            var fileEmailClient = new FileEmailClient(_tempDirPath);
            var emailData = new CustomEmailData(new EmailAddress("eugene@onegin.name"));
            emailData.From = new EmailAddress("tatyana@larina.name");

            // Act
            await fileEmailClient.SendAsync(emailData, "I write this to you - what more can be said?", default(CancellationToken));

            // Assert
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
