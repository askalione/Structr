using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Moq;
using Structr.AspNetCore.Mvc;
using Structr.AspNetCore.Rewrite;
using System;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Rewrite
{
    public class RedirectToLowercaseRuleTests
    {
        [Fact]
        public void Ctor()
        {
            // Act
            Action act = () => new RedirectToLowercaseRule(x => true, 301);

            // Assert
            act.Should().NotThrow<ArgumentNullException>();
        }

        [Fact]
        public void Ctor_throws_when_filter_is_null()
        {
            // Act
            Action act = () => new RedirectToLowercaseRule(null, 301);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        //[Theory]
        //[InlineData("", "gEt", true, "")]
        //public void ApplyRule(string url, string method, bool filter, string expected)
        //{
        //    // maybe need this: var client = new WebApplicationFactory<Startup>().CreateClient();

        //    // Arrange
        //    var rule = new RedirectToLowercaseRule(x => filter, 301);
        //    var httpRequest = new Mock<HttpRequest>();
        //    httpRequest.

        //    var httpContext = new Mock<HttpContext>();
        //    httpContext.SetupGet(x => x.Request).Returns(httpRequest.Object);
        //    var rewriteContext = new RewriteContext();
        //    rewriteContext.HttpContext = httpContext.Object;

        //    // Act
        //    rule.ApplyRule(rewriteContext);

        //    // Assert
        //    rewriteContext.HttpContext.Response.Headers[HeaderNames.Location].Should().Be("");
        //}
    }
}
