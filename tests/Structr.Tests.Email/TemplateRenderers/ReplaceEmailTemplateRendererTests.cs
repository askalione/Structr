using Structr.Email.TemplateRenderers;
using Structr.Tests.Email.TestUtils;

namespace Structr.Tests.Email.TemplateRenderers
{
    public class ReplaceEmailTemplateRendererTests
    {
        [Fact]
        public async Task RenderAsync()
        {
            // Arrange
            var renderer = new ReplaceEmailTemplateRenderer();
            var template = "Letter of {{From}} to {{To}}.";
            var model = new CustomModel();

            // Act
            var result = await renderer.RenderAsync(template, model);

            // Assert
            result.Should().Be("Letter of Tatyana to Onegin.");
        }
    }
}
