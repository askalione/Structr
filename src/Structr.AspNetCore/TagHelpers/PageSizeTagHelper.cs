using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.TagHelpers
{
    /// <summary>
    /// A <see cref="TagHelper"/> that creates dropdown menu for page size changing.
    /// </summary>
    [HtmlTargetElement("page-size", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageSizeTagHelper : TagHelper
    {
        /// <summary>
        /// Options influating appearance of dropdown menu.
        /// </summary>
        [HtmlAttributeName("asp-options")]
        public PageSizeOptions Options { get; set; }

        /// <summary>
        /// An actual instance of <see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IUrlHelper _urlHelper;

        /// <summary>
        /// Creates an instance of <see cref="PageSizeTagHelper"/>.
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PageSizeTagHelper(IUrlHelper urlHelper)
        {
            if (urlHelper == null)
            {
                throw new ArgumentNullException(nameof(urlHelper));
            }

            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Options == null)
            {
                Options = new PageSizeOptions();
            }

            if (Options.ItemsPerPage == null || Options.ItemsPerPage.Any() == false)
            {
                output.SuppressOutput();
                return;
            }

            var items = new List<string>();
            var request = ViewContext.HttpContext.Request;
            var action = ViewContext.RouteData.Values["action"].ToString();
            var controller = ViewContext.RouteData.Values["controller"].ToString();
            var pageSizeRouteParamName = !string.IsNullOrEmpty(Options.PageSizeRouteParamName) ? Options.PageSizeRouteParamName : "pagesize";
            var allItemsFormat = !string.IsNullOrEmpty(Options.AllItemsFormat) ? Options.AllItemsFormat : "All";

            TagBuilder menu = new TagBuilder("div");
            menu.AddCssClass("dropdown-menu");
            menu.AddCssClass("dropdown-menu-" + Options.DropdownMenuAlign.ToString().ToLower());

            foreach (int value in Options.ItemsPerPage.Where(x => x > 0))
            {
                var url = _urlHelper.Action(action, controller, request.Query.ToRouteValueDictionary(pageSizeRouteParamName, value));
                TagBuilder item = new TagBuilder("a");
                item.AddCssClass("dropdown-item");
                item.Attributes.Add("href", url);
                item.InnerHtml.Append(value.ToString());
                menu.InnerHtml.AppendHtml(item);
            }

            if (Options.ItemsPerPage.Any(x => x <= 0))
            {
                TagBuilder divider = new TagBuilder("div");
                divider.AddCssClass("dropdown-divider");
                menu.InnerHtml.AppendHtml(divider);

                var url = _urlHelper.Action(action, controller, request.Query.ToRouteValueDictionary(pageSizeRouteParamName, 0));
                TagBuilder item = new TagBuilder("a");
                item.AddCssClass("dropdown-item");
                item.Attributes.Add("href", url);
                item.InnerHtml.Append(allItemsFormat);
                menu.InnerHtml.AppendHtml(item);
            }

            TagBuilder toggle = new TagBuilder("button");
            toggle.AddCssClass("dropdown-toggle");
            if (string.IsNullOrEmpty(Options.DropdownToggleCssClass) == false)
            {
                toggle.AddCssClass(Options.DropdownToggleCssClass);
            }
            toggle.Attributes.Add("type", "button");
            if (string.IsNullOrEmpty(Options.DropdownToggleAttribute) == false) 
            {
                toggle.Attributes.Add(Options.DropdownToggleAttribute, "dropdown");
            }
            toggle.InnerHtml.Append(Options.DefaultPageSize > 0 ? Options.DefaultPageSize.ToString() : allItemsFormat);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("class", "page-size dropdown dropup" + (string.IsNullOrEmpty(Options.DropdownCssClass) == false ? " " + Options.DropdownCssClass : ""));
            output.Content.AppendHtml(toggle);
            output.Content.AppendHtml(menu);
        }
    }

    /// <summary>
    /// Defines the alignment of page size dropdown menu.
    /// </summary>
    public enum PageSizeDropdownMenuAlign
    {
        Left,
        Right
    }

    /// <summary>
    /// Defines parameters of element controlling page size.
    /// </summary>
    public class PageSizeOptions
    {
        /// <summary>
        /// A text to show for menu element corresponding to visualising of all items.
        /// </summary>
        public string AllItemsFormat { get; set; }

        /// <summary>
        /// Allign of dropdown menu elements.
        /// </summary>
        public PageSizeDropdownMenuAlign DropdownMenuAlign { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string ContainerCssClass { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string DropdownCssClass { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string DropdownToggleCssClass { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public string DropdownToggleAttribute { get; set; }

        /// <summary>
        /// List of possible values of page sizes.
        /// </summary>
        public IEnumerable<int> ItemsPerPage { get; set; }

        /// <summary>
        /// Name of route parameter containing the page size value.
        /// </summary>
        public string PageSizeRouteParamName { get; set; }

        /// <summary>
        /// Page size choosed by default.
        /// </summary>
        public int DefaultPageSize { get; set; }

        /// <summary>
        /// Creates an instance of <see cref="PageSizeOptions"/>.
        /// </summary>
        /// <remarks>
        /// New instance has the following default vaules:
        /// <br/><see cref="AllItemsFormat"/> - "All",
        /// <br/><see cref="DropdownMenuAlign"/> - <see cref="PageSizeDropdownMenuAlign.Right"/>
        /// <br/><see cref="DropdownToggleCssClass"/> - "btn btn-secondary"
        /// <br/><see cref="DropdownToggleAttribute"/> - "data-toggle"
        /// <br/><see cref="PageSizeRouteParamName"/> - "pagesize"
        /// </remarks>
        public PageSizeOptions()
        {
            AllItemsFormat = "All";
            DropdownMenuAlign = PageSizeDropdownMenuAlign.Right;
            DropdownToggleCssClass = "btn btn-secondary";
            DropdownToggleAttribute = "data-toggle";
            PageSizeRouteParamName = "pagesize";
        }
    }
}
