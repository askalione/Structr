using Structr.Email;

namespace Structr.Tests.Email
{
    public class EmailAttachmentTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new EmailAttachment("readme.txt");

            // Assert
            result.FileName.Should().Be("readme.txt");
            result.ContentType.Should().BeNull();
            result.Content.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_fileName_is_null_or_empty(string fileName)
        {
            // Act
            Action act = () => new EmailAttachment(fileName);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_contentType()
        {
            // Act
            var result = new EmailAttachment("readme.txt", "text/plain");

            // Assert
            result.FileName.Should().Be("readme.txt");
            result.ContentType.Should().Be("text/plain");
            result.Content.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_contentType_is_null_or_empty(string contentType)
        {
            // Act
            Action act = () => new EmailAttachment("readme.txt", contentType);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_with_content()
        {
            // Arrange
            Stream stream = new MemoryStream();

            // Act
            var result = new EmailAttachment(stream, "readme.txt", "text/plain");

            // Assert
            result.FileName.Should().Be("readme.txt");
            result.ContentType.Should().Be("text/plain");
            result.Content.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_content_is_null(Stream content)
        {
            // Act
            Action act = () => new EmailAttachment(content, "readme.txt", "text/plain");

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
