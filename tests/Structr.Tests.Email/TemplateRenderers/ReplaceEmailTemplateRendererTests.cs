using Structr.Email.TemplateRenderers;

namespace Structr.Tests.Email.TemplateRenderers
{
    public class ReplaceEmailTemplateRendererTests
    {
        class CustomModel
        {
            public string Name { get; set; } = "Custom name";
            public string Description { get; set; } = "Custom description";
        }

        [Fact]
        public async Task RenderAsync()
        {
            // Arrange
            var renderer = new ReplaceEmailTemplateRenderer();
            var template = "{{Name}} <{{Description}}>";
            var model = new CustomModel();

            // Act
            var result = await renderer.RenderAsync(template, model);

            // Assert
            result.Should().Be("Custom name <Custom description>");
        }
    }
}
