using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.TagHelpers;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.TagHelpers
{
    public class TagHelperOutputExtensionsTests
    {
        [Fact]
        public void AddClass()
        {
            // Arrange
            var tagHelperOutput = new TagHelperOutput("a", new TagHelperAttributeList(), (useCachedResult, htmlEncoder) =>
                Task.Factory.StartNew<TagHelperContent>(() => new DefaultTagHelperContent()));

            // Act
            tagHelperOutput.AddClass("test-css-class");
            tagHelperOutput.AddClass("test-css-class-2");

            // Assert
            tagHelperOutput.Attributes.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("class");
                    x.Value.Should().BeEquivalentTo(new HtmlString("test-css-class test-css-class-2"));
                });
        }
    }
}
