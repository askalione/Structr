using Structr.Email;
using Structr.Email.Razor;
using Structr.Tests.Email.Razor.TestUtils;

namespace Structr.Tests.Email.Razor
{
    public class RazorEmailTemplateRendererTests
    {
        [Fact]
        public void Ctor()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("tatyana@larina.name"));
            options.TemplateRootPath = TestDataPath.ContentRootPath;

            // Act
            var result = new RazorEmailTemplateRenderer(options);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(null)]
        public void Ctor_throws_when_options_is_null(EmailOptions options)
        {
            // Act
            Action act = () => new RazorEmailTemplateRenderer(options);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Ctor_throws_when_templateRootPath_is_null(string templateRootPath)
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("tatyana@larina.name"));
            options.TemplateRootPath = templateRootPath;

            // Act
            Action act = () => new RazorEmailTemplateRenderer(options);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>();
        }

        [Fact]
        public void Ctor_throws_when_templateRootPath_does_not_exist()
        {
            // Arrange
            var options = new EmailOptions(new EmailAddress("tatyana@larina.name"));
            options.TemplateRootPath = TestDataPath.Combine("NotExistFolder");

            // Act
            Action act = () => new RazorEmailTemplateRenderer(options);

            // Assert
            act.Should().ThrowExactly<DirectoryNotFoundException>();
        }

        [Fact]
        public async Task RenderAsync()
        {
            // Arrange            
            var options = new EmailOptions(new EmailAddress("tatyana@larina.name"));
            options.TemplateRootPath = TestDataPath.ContentRootPath;
            var renderer = new RazorEmailTemplateRenderer(options);
            var template = File.ReadAllText(TestDataPath.Combine("Letter of Tatyana to Onegin Razor Template.txt"));
            var model = new CustomModel();

            // Act
            string result = await renderer.RenderAsync(template, model);

            // Assert
            result.Should().StartWith("Letter of Tatyana to Onegin.");
        }
    }
}
