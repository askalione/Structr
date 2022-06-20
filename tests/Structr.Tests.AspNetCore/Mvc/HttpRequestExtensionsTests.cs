using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Structr.AspNetCore.Internal;
using Structr.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    /// <summary>
    /// Defines extension methods on <see cref="HttpRequest"/>.
    /// </summary>
    public class HttpRequestExtensions
    {
        [Theory]
        [InlineData("x-requested-with", true)]
        [InlineData("some-other-header", false)]
        public void IsValidForRequest(string headerName, bool expected)
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
            // ???: or use integration test with test request object builded natively.
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

        [Theory]
        [InlineData(null, "/Users/Edit/1")]
        [InlineData("/Users/Details/1", "/Users/Details/1")]
        public void GetReferrer(string referrer, string expected)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            if (referrer != null)
            {
                httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
                httpContext.Request.Form =
                    new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> {
                        { ReferrerConstants.Key, referrer }
                    });
            }

            // Act
            var result = httpContext.Request.GetReferrer("/Users/Edit/1");

            // Assert
            result.Should().Be(expected);
        }
    }
}
