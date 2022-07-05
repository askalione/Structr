using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Structr.AspNetCore.Referrer;
using Structr.Tests.AspNetCore.TestUtils;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Xunit;

namespace Structr.Tests.AspNetCore.Referrer
{
    public class ReferrerTagHelperTests
    {
        [Theory]
        [InlineData(// No referrer from headers and referrer from form - provided referrer link generated.
            "",
            null,
            "/Users/Edit",
            "http://localhost:8080/Users/Details?id=1",
            "http://localhost:8080/Users/Details?id=1"
            )]
        [InlineData(// Referrer from headers overrides provided link.
            "http://localhost:8080/Users/OverallInfo?id=1",
            null,
            "/Users/Edit",
            "http://localhost:8080/Users/Details?id=1",
            "http://localhost:8080/Users/OverallInfo?id=1")]
        [InlineData(// Referrer from form overrides provided link.
            "",
            "http://localhost:8080/Users/OverallInfo?id=1",
            "/Users/Edit",
            "http://localhost:8080/Users/Details?id=1",
            "http://localhost:8080/Users/OverallInfo?id=1")]
        [InlineData(// Referrer from form overrides both referrer from headers and provided link.
            "http://localhost:8080/Users/OverallInfo?id=1",
            "http://localhost:8080/Users/NewOverallInfo?id=1",
            "/Users/Edit",
            "http://localhost:8080/Users/Details?id=1",
            "http://localhost:8080/Users/NewOverallInfo?id=1")]
        [InlineData(// Referrer from headers equals page's url it self - provided referrer link generated.
            "http://localhost:8080/Users/OverallInfo?id=1",
            null,
            "/Users/OverallInfo",
            "http://localhost:8080/Users/Details?id=1",
            "http://localhost:8080/Users/Details?id=1")]
        [InlineData(// Referrer from form equals page's url it self but despite of this it still will be used.
            "",
            "http://localhost:8080/Users/OverallInfo?id=1",
            "/Users/OverallInfo",
            "http://localhost:8080/Users/Details?id=1",
            "http://localhost:8080/Users/OverallInfo?id=1")]
        public void Process(string referrerFromHeaders, string? referrerFromForm, string requestControllerAction, string providedReferrer, string expected)
        {
            var context = TagHelperContextFactory.Create();
            var output = TagHelperOutputFactory.Create();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add("Referer", referrerFromHeaders);
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("localhost:8080");
            httpContext.Request.Path = requestControllerAction;
            httpContext.Request.QueryString = new QueryString("?id=1");

            if (referrerFromForm != null)
            {
                httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
                httpContext.Request.Form =
                    new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues> {
                        { ReferrerDefaults.Key, referrerFromForm }
                    });
            }

            var tagHelper = new ReferrerTagHelper()
            {
                ViewContext = new ViewContext()
                {
                    HttpContext = httpContext,
                },
                Referrer = providedReferrer
            };

            // Act
            tagHelper.Process(context, output);

            // Assert
            output.Attributes.Should().SatisfyRespectively(
            x =>
            {
                x.Name.Should().Be("href");
                x.Value.Should().Be(expected);
            });
            var sw = new StringWriter();
            output.PostElement.WriteTo(sw, HtmlEncoder.Default);
            sw.ToString().Should().Be(@$"<input name=""__Referrer"" type=""hidden"" value=""{expected}""></input>");
        }
    }
}
