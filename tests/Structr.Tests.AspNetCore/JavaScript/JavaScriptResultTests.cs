using FluentAssertions;
using Structr.AspNetCore.JavaScript;
using Xunit;

namespace Structr.Tests.AspNetCore.JavaScript
{
    public class JavaScriptResultTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            var result = new JavaScriptResult("alert('Hello World!')");

            // Assert
            result.ContentType.Should().Be("application/javascript");
            result.Content.Should().Be("alert('Hello World!')");
        }
    }
}
