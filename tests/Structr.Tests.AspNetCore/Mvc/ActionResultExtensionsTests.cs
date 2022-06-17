using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Structr.AspNetCore.JavaScript;
using Structr.AspNetCore.Mvc;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class ActionResultExtensionsTests
    {
        [Fact]
        public void AddJavaScriptAlertTest()
        {
            // Act
            var result = new ContentResult().AddJavaScriptAlert(new JavaScriptAlert("Type1", "Message1"));

            // Assert
            result.Should().BeOfType<JavaScriptAlertResult>();
        }
    }
}
