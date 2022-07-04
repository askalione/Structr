using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Structr.AspNetCore.Http;
using Xunit;

namespace Structr.Tests.AspNetCore.Http
{
    public class HttpRequestExtensionsTests
    {
        [Theory]
        [InlineData("x-requested-with", true)]
        [InlineData("some-other-header", false)]
        public void IsAjaxRequest(string headerName, bool expected)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add(headerName, "XMLHttpRequest");

            // Act
            var result = httpContext.Request.IsAjaxRequest();

            // Assert
            result.Should().Be(expected);
        }

        [Fact]
        public void GetAbsoluteUri()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:8080");
            httpContext.Request.Path = "/Users/Details";
            httpContext.Request.QueryString = new QueryString("?id=1");

            // Act
            var result = httpContext.Request.GetAbsoluteUri();

            // Assert
            result.Should().Be("http://localhost:8080/Users/Details?id=1");
        }
    }
}
