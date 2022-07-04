using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Structr.Tests.AspNetCore.TestUtils
{
    internal static class TagHelperContextFactory
    {
        public static TagHelperContext Create(string tagName = "a")
        {
            var context = new TagHelperContext(
                tagName: tagName,
                allAttributes: new TagHelperAttributeList(Enumerable.Empty<TagHelperAttribute>()),
                items: new Dictionary<object, object>(),
                uniqueId: "test");
            return context;
        }
    }
}
