using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Structr.AspNetCore.Rewrite;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Rewrite
{
    public class RedirectToLowercaseTrailingSlashRuleTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new RedirectToLowercaseTrailingSlashRule(x => true, 301);

            // Assert
            act.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_filter_is_null()
        {
            // Act
            Action act = () => new RedirectToLowercaseTrailingSlashRule(null, 301);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", true, "http://localhost:8080/users/index/?id=1")]
        [InlineData("/Users/Index/", "gEt", true, "http://localhost:8080/users/index/?id=1")]
        [InlineData("/users/index", "gEt", true, "http://localhost:8080/users/index/?id=1")]
        [InlineData("/users/index/", "gEt", true, "")] // No need for redirect.
        [InlineData("/Users/Index", "gEt", false, "")] // Filter prohibits redirect.
        [InlineData("/Users/Index", "POST", true, "")] // Only GET should be redirected.
        public void ApplyRule(string controllerAction, string method, bool filter, string expected)
        {
            // Arrange
            var rule = new RedirectToLowercaseTrailingSlashRule(x => filter, 301);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Method = method;
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:8080");
            httpContext.Request.Path = controllerAction;
            httpContext.Request.QueryString = new QueryString("?id=1");

            var rewriteContext = new RewriteContext { HttpContext = httpContext };

            // Act
            rule.ApplyRule(rewriteContext);

            // Assert
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 301);
        }
    }
}
