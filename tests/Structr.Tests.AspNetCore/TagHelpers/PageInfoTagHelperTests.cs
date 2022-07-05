using FluentAssertions;
using Structr.AspNetCore.TagHelpers;
using Structr.Collections;
using Structr.Tests.AspNetCore.TestUtils;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using Xunit;

namespace Structr.Tests.AspNetCore.TagHelpers
{
    public class PageInfoTagHelperTests
    {
        [Fact]
        public void Process()
        {
            var context = TagHelperContextFactory.Create();
            var output = TagHelperOutputFactory.Create();

            var tagHelper = new PageInfoTagHelper()
            {
                Options = new PageInfoOptions
                {
                    PagedEnumerable = new PagedList<string>(new[] { "Five", "Six", "Seven", "Eight" }, 12, 2, 4),
                    Format = "Test Page {0} of {1}. Showing items {2} through {3} of {4}."
                }
            };

            // Act
            tagHelper.Process(context, output);

            // Assert
            var sw = new StringWriter();
            output.Content.WriteTo(sw, HtmlEncoder.Default);
            sw.ToString().Should().Be("Test Page 2 of 3. Showing items 5 through 8 of 12.");

            output.Attributes
                .FirstOrDefault(x => x.Name == "class")?
                .Value.Should().Be("page-info");
        }
    }
}
