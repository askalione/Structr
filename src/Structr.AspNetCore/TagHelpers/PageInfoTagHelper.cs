using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.Collections;

namespace Structr.AspNetCore.TagHelpers
{
    [HtmlTargetElement("page-info", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageInfoTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-options")]
        public PageInfoOptions Options { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Options == null)
            {
                Options = new PageInfoOptions();
            }

            var pagedList = Options.PagedList;

            if (pagedList == null)
            {
                output.SuppressOutput();
                return;
            }

            var pageInfo = string.Format(Options.Format,
                pagedList.PageNumber,
                pagedList.TotalPages,
                pagedList.TotalItems > 0 ? pagedList.FirstItemOnPage : 0,
                pagedList.LastItemOnPage,
                pagedList.TotalItems);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("class", "page-info");
            output.Content.Append(pageInfo);
        }
    }

    public class PageInfoOptions
    {
        public IPagedList PagedList { get; set; }
        public string Format { get; set; }

        public PageInfoOptions()
        {
            Format = "Page {0} of {1}. Showing items {2} through {3} of {4}.";
        }
    }
}
