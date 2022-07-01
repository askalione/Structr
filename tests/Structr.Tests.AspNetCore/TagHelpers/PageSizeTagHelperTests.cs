using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Structr.Tests.AspNetCore.TestWebApp;
using Xunit;

namespace Structr.Tests.AspNetCore.TagHelpers
{
    public class PageSizeTagHelperTests
    {
        // TODO: more PageSizeTagHelperTests

        [Fact]
        public async void Process()
        {
            // Arrange
            var client = new WebApplicationFactory<Startup>().CreateClient();

            // Act
            var response = await client.GetAsync($"/TagHelpers/PageSizeTagHelper");

            // Assert
            response.Should().BeSuccessful();
            var result = await response.Content.ReadAsStringAsync();
            result.Should().Be(@"<div class=""page-size dropdown dropup""><button class=""dropdown-toggle test-dropdown-toggle-css-class"" data-toggle=""dropdown"" type=""button"">Show all</button><div class=""dropdown-menu dropdown-menu-right""><a class=""dropdown-item"" href=""/TagHelpers/PageSizeTagHelper?pagesize=3"">3</a><a class=""dropdown-item"" href=""/TagHelpers/PageSizeTagHelper?pagesize=6"">6</a><a class=""dropdown-item"" href=""/TagHelpers/PageSizeTagHelper?pagesize=10"">10</a></div></div>");
        }
    }
}
