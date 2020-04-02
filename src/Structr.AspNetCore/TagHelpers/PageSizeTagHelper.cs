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
    [HtmlTargetElement("page-size", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PageSizeTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-options")]
        public PageSizeOptions Options { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IUrlHelper _urlHelper;

        public PageSizeTagHelper(IUrlHelper urlHelper)
        {
            if (urlHelper == null)
                throw new ArgumentNullException(nameof(urlHelper));

            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Options == null)
            {
                Options = new PageSizeOptions();
            }

            if (Options.ItemsPerPage == null || !Options.ItemsPerPage.Any())
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
            if (!string.IsNullOrEmpty(Options.DropdownToggleCssClass))
            {
                toggle.AddCssClass(Options.DropdownToggleCssClass);
            }
            toggle.Attributes.Add("type", "button");
            toggle.Attributes.Add("data-toggle", "dropdown");
            toggle.InnerHtml.Append((Options.DefaultPageSize > 0 ? Options.DefaultPageSize.ToString() : allItemsFormat));

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("class", "page-size dropdown dropup" + (!string.IsNullOrEmpty(Options.DropdownCssClass) ? " " + Options.DropdownCssClass : ""));
            output.Content.AppendHtml(toggle);
            output.Content.AppendHtml(menu);
        }
    }

    public enum PageSizeDropdownMenuAlign
    {
        Left,
        Right
    }

    public class PageSizeOptions
    {
        public string AllItemsFormat { get; set; }
        public PageSizeDropdownMenuAlign DropdownMenuAlign { get; set; }
        public string ContainerCssClass { get; set; }
        public string DropdownCssClass { get; set; }
        public string DropdownToggleCssClass { get; set; }
        public IEnumerable<int> ItemsPerPage { get; set; }
        public string PageSizeRouteParamName { get; set; }
        public int DefaultPageSize { get; set; }

        public PageSizeOptions()
        {
            AllItemsFormat = "All";
            DropdownMenuAlign = PageSizeDropdownMenuAlign.Right;
            DropdownToggleCssClass = "btn btn-secondary";
            PageSizeRouteParamName = "pagesize";
        }
    }
}
