using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Structr.AspNetCore.Referrer;
using System.Collections.Generic;
using Xunit;

namespace Structr.Tests.AspNetCore.Referrer
{
    public class ReferrerHttpRequestExtensionsTests
    {
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
                        { ReferrerDefaults.Key, referrer }
                    });
            }

            // Act
            var result = httpContext.Request.GetReferrer("/Users/Edit/1");

            // Assert
            result.Should().Be(expected);
        }
    }
}
