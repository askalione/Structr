using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Structr.AspNetCore.Rewrite;
using Structr.Tests.AspNetCore._TestUtils;
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
        public void ApplyRule(string path, string method, bool filter, string expected)
        {
            // Arrange
            var rule = new RedirectToLowercaseTrailingSlashRule(x => filter, 307);
            var rewriteContext = RewriteContextFactory.Create(method, path);

            // Act
            rule.ApplyRule(rewriteContext);

            // Assert
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 307);
        }
    }
}
