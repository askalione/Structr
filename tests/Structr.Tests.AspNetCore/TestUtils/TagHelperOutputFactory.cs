using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace Structr.Tests.AspNetCore.TestUtils
{
    internal static class TagHelperOutputFactory
    {
        public static TagHelperOutput Create(string tagName = "a", string content = "Some content")
        {
            var output = new TagHelperOutput(
                tagName,
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (useCachedResult, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetContent(content);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
            output.Content.SetContent(string.Empty);
            return output;
        }
    }
}
