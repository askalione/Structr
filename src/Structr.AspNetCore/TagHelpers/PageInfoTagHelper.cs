using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.Collections;

namespace Structr.AspNetCore.TagHelpers
{
    /// <summary>
    /// A <see cref="TagHelper"/> allowing to organize info-text about pagination in a simple manner.
    /// </summary>
    [HtmlTargetElement("page-info", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageInfoTagHelper : TagHelper
    {
        /// <summary>
        /// An instance of <see cref="PageInfoOptions"/> to be used.
        /// </summary>
        [HtmlAttributeName("asp-options")]
        public PageInfoOptions Options { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Options == null)
            {
                Options = new PageInfoOptions();
            }

            var pagedList = Options.PagedEnumerable;
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

    /// <summary>
    /// Options to be used while creating info-text about pagination.
    /// </summary>
    public class PageInfoOptions
    {
        /// <summary>
        /// An instance of <see cref="IPagedEnumerable"/> to get information about pagination from.
        /// </summary>
        public IPagedEnumerable PagedEnumerable { get; set; }

        /// <summary>
        /// Info-text format string.
        /// </summary>
        /// <remarks>
        /// It uses string-interpolation with following 5 parameters:
        /// <br/>0 - <see cref="IPagedEnumerable.PageNumber"/>
        /// <br/>1 - <see cref="IPagedEnumerable.TotalPages"/>
        /// <br/>2 - <see cref="IPagedEnumerable.FirstItemOnPage"/>
        /// <br/>3 - <see cref="IPagedEnumerable.LastItemOnPage"/>
        /// <br/>4 - <see cref="IPagedEnumerable.TotalItems"/>
        /// </remarks>
        public string Format { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PageInfoOptions"/> with default Format:
        /// <c>'Page {0} of {1}. Showing items {2} through {3} of {4}.'</c>
        /// </summary>
        public PageInfoOptions()
        {
            Format = "Page {0} of {1}. Showing items {2} through {3} of {4}.";
        }
    }
}
