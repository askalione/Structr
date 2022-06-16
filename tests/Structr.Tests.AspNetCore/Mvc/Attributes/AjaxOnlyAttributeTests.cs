using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Structr.AspNetCore.Mvc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc.Attributes
{
    public class AjaxOnlyAttributeTests
    {
        [Theory]
        [InlineData("x-requested-with", true)]
        [InlineData("some-other-header", false)]
        public void IsValidForRequest(string headerName, bool expected)
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers.Add(headerName, "XMLHttpRequest");
            var routeContext = new RouteContext(httpContext);

            // Act
            var result = new AjaxOnlyAttribute().IsValidForRequest(routeContext, new ActionDescriptor());

            // Assert
            result.Should().Be(expected);
        }
    }
}
