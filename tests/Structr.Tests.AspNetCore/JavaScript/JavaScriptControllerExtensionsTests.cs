using FluentAssertions;
using Structr.AspNetCore.JavaScript;
using Structr.Tests.AspNetCore.TestUtils;
using Xunit;

namespace Structr.Tests.AspNetCore.JavaScript
{
    public class JavaScriptControllerExtensionsTests
    {
        [Fact]
        public void JavaScript()
        {
            // Act
            var result = new TestController().JavaScript("alert('Hello World!')");

            // Assert
            result.ContentType.Should().Be("application/javascript");
            result.Content.Should().Be("alert('Hello World!')");
        }

        [Theory]
        [InlineData("http://example.com", "/")]
        [InlineData("/Users/Details/1", "/Users/Details/1")]
        public void AjaxLocalRedirect(string url, string expected)
        {
            // Act
            var result = ControllerFactory.CreateController(out _).AjaxLocalRedirect(url);

            // Assert
            result.Url.Should().Be(expected);
        }

        [Fact]
        public void AjaxRedirect()
        {
            // Act
            var result = new TestController().AjaxRedirect("/Users/Details/1");

            // Assert
            result.Url.Should().Be("/Users/Details/1");
        }
    }
}
