using FluentAssertions;
using Structr.AspNetCore.Referrer;
using Structr.Tests.AspNetCore.TestUtils;
using System;
using Xunit;

namespace Structr.Tests.AspNetCore.Referrer
{
    public class ReferrerControllerExtensionsTests
    {
        [Theory]
        [InlineData(null, "/Users/Edit/1")]
        [InlineData("/Users/Details/1", "/Users/Details/1")]
        public void RedirectToReferrer(string reffrer, string expected)
        {
            // Arrange
            var controller = ControllerFactory.CreateController(out _, reffrer);

            // Act
            var result = controller.RedirectToReferrer("/Users/Edit/1");

            // Assert
            result.Url.Should().Be(expected);
        }

        [Fact]
        public void RedirectToReferrer_throws_when_url_is_null_or_empty()
        {
            // Act
            Action act = () => new TestController().RedirectToReferrer(null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }
    }
}
