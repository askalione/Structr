using FluentAssertions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Structr.AspNetCore.Rewrite;
using Structr.Tests.AspNetCore.TestUtils;
using System.Linq;
using Xunit;

namespace Structr.Tests.AspNetCore.Rewrite
{
    public class RewriteOptionsExtensionsTests
    {
        [Fact]
        public void ApplyRule()
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToLowercase();

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToLowercaseRule>();
                });
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", "http://localhost:8080/users/index?id=1")]
        [InlineData("/users/index", "gEt", "")] // No need for redirect.
        [InlineData("/Users/Index", "POST", "")] // Only GET should be redirected.
        public void AddRedirectToLowercase(string path, string method, string expected)
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToLowercase();

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToLowercaseRule>();
                });

            var rewriteContext = RewriteContextFactory.Create(method, path);
            opts.Rules.Single().ApplyRule(rewriteContext);
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 301);
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", true, "http://localhost:8080/users/index?id=1")]
        [InlineData("/users/index", "gEt", true, "")] // No need for redirect.
        [InlineData("/Users/Index", "gEt", false, "")] // Filter prohibits redirect.
        [InlineData("/Users/Index", "POST", true, "")] // Only GET should be redirected.
        public void AddRedirectToLowercase_with_params(string path, string method, bool filter, string expected)
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToLowercase(x => filter, 307);

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToLowercaseRule>();
                });

            var rewriteContext = RewriteContextFactory.Create(method, path);
            opts.Rules.Single().ApplyRule(rewriteContext);
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 307);
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", "http://localhost:8080/Users/Index/?id=1")]
        [InlineData("/Users/Index/", "gEt", "")] // No need for redirect.
        [InlineData("/Users/Index", "POST", "")] // Only GET should be redirected.
        public void AddRedirectToTrailingSlash(string path, string method, string expected)
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToTrailingSlash();

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToTrailingSlashRule>();
                });

            var rewriteContext = RewriteContextFactory.Create(method, path);
            opts.Rules.Single().ApplyRule(rewriteContext);
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 301);
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", true, "http://localhost:8080/Users/Index/?id=1")]
        [InlineData("/Users/Index/", "gEt", true, "")] // No need for redirect.
        [InlineData("/Users/Index", "gEt", false, "")] // Filter prohibits redirect.
        [InlineData("/Users/Index", "POST", true, "")] // Only GET should be redirected.
        public void AddRedirectToTrailingSlash_with_params(string path, string method, bool filter, string expected)
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToTrailingSlash(x => filter, 307);

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToTrailingSlashRule>();
                });

            var rewriteContext = RewriteContextFactory.Create(method, path);
            opts.Rules.Single().ApplyRule(rewriteContext);
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 307);
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", "http://localhost:8080/users/index/?id=1")]
        [InlineData("/Users/Index/", "gEt", "http://localhost:8080/users/index/?id=1")]
        [InlineData("/users/index", "gEt", "http://localhost:8080/users/index/?id=1")]
        [InlineData("/users/index/", "gEt", "")] // No need for redirect.
        [InlineData("/Users/Index", "POST", "")] // Only GET should be redirected.
        public void AddRedirectToLowercaseTrailingSlash(string path, string method, string expected)
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToLowercaseTrailingSlash();

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToLowercaseTrailingSlashRule>();
                });

            var rewriteContext = RewriteContextFactory.Create(method, path);
            opts.Rules.Single().ApplyRule(rewriteContext);
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 301);
        }

        [Theory]
        [InlineData("/Users/Index", "gEt", true, "http://localhost:8080/users/index/?id=1")]
        [InlineData("/Users/Index/", "gEt", true, "http://localhost:8080/users/index/?id=1")]
        [InlineData("/users/index", "gEt", true, "http://localhost:8080/users/index/?id=1")]
        [InlineData("/users/index/", "gEt", true, "")] // No need for redirect.
        [InlineData("/Users/Index", "gEt", false, "")] // Filter prohibits redirect.
        [InlineData("/Users/Index", "POST", true, "")] // Only GET should be redirected.
        public void AddRedirectToLowercaseTrailingSlash_with_params(string path, string method, bool filter, string expected)
        {
            // Arrange
            var opts = new RewriteOptions();

            // Act
            opts.AddRedirectToLowercaseTrailingSlash(x => filter, 307);

            // Assert
            opts.Rules.Should().SatisfyRespectively(
                x =>
                {
                    x.Should().BeOfType<RedirectToLowercaseTrailingSlashRule>();
                });

            var rewriteContext = RewriteContextFactory.Create(method, path);
            opts.Rules.Single().ApplyRule(rewriteContext);
            rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].ToString().Should().Be(expected);
            rewriteContext.HttpContext.Response.StatusCode.Should().Be(string.IsNullOrEmpty(expected) ? 200 : 307);
        }
    }
}
