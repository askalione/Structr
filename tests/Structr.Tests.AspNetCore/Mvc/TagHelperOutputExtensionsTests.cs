using FluentAssertions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace Structr.Tests.AspNetCore.Mvc
{
    public class TagHelperOutputExtensionsTests
    {
        [Fact]
        public void AddClass()
        {
            // Arrange
            var th = new TagHelperOutput("a", new TagHelperAttributeList(), (useCachedResult, htmlEncoder) =>
                Task.Factory.StartNew<TagHelperContent>(() => new DefaultTagHelperContent()));

            // Act
            th.AddClass("SomeTestClass");
            th.AddClass("SomeTestClass2");

            // Assert
            th.Attributes.Should().SatisfyRespectively(
                x => {
                    x.Name.Should().Be("class");
                    x.Value.Should().BeEquivalentTo(new HtmlString("SomeTestClass SomeTestClass2"));
                });
        }
    }
}
