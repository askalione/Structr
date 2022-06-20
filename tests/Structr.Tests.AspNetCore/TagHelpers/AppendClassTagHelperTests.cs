using FluentAssertions;
using Structr.AspNetCore.TagHelpers;
using Structr.Tests.AspNetCore._TestUtils;
using System.Linq;
using Xunit;

namespace Structr.Tests.AspNetCore.TagHelpers
{
    public class AppendClassTagHelperTests
    {
        [Theory]
        [InlineData(true, "test-css-class")]
        [InlineData(false, null)]
        public void Process(bool append, string expected)
        {
            var context = TagHelperContextFactory.Create();
            var output = TagHelperOutputFactory.Create();

            var tagHelper = new AppendClassTagHelper()
            {
                Append = append,
                Class = "test-css-class",
            };

            // Act
            tagHelper.Process(context, output);

            // Assert
            output.Attributes
                .FirstOrDefault(x => x.Name == "class")?
                .Value.Should().Be(expected);
        }
    }
}
