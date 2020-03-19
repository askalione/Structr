using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Structr.AspNetCore.Mvc;
using Structr.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Structr.AspNetCore.TagHelpers
{
    [HtmlTargetElement("pagination", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PaginationTagHelper : TagHelper
    {
        [HtmlAttributeName("asp-options")]
        public PaginationOptions Options { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IUrlHelper _urlHelper;

        public PaginationTagHelper(IUrlHelper urlHelper)
        {
            if (urlHelper == null)
                throw new ArgumentNullException(nameof(urlHelper));

            _urlHelper = urlHelper;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Options == null)
            {
                Options = new PaginationOptions();
            }

            var pagedList = Options.PagedList;

            if (pagedList == null)
            {
                output.SuppressOutput();
                return;
            }

            if (Options.Display == PaginationDisplayMode.Never || (Options.Display == PaginationDisplayMode.IfNeeded && pagedList.TotalPages <= 1))
            {
                output.SuppressOutput();
                return;
            }


            Func<int, string> pageUrlGenerator = Options.PageUrlGenerator ?? (pageNumber => AppendPageNumberToPageUrl(pageNumber));

            var listItemLinks = new List<TagBuilder>();

            var firstPageToDisplay = 1;
            var lastPageToDisplay = pagedList.TotalPages;
            var pageNumbersToDisplay = lastPageToDisplay;
            if (Options.MaximumPageNumbersToDisplay.HasValue && pagedList.TotalPages > Options.MaximumPageNumbersToDisplay)
            {
                var maxPageNumbersToDisplay = Options.MaximumPageNumbersToDisplay.Value;
                firstPageToDisplay = pagedList.PageNumber - maxPageNumbersToDisplay / 2;
                if (firstPageToDisplay < 1)
                    firstPageToDisplay = 1;
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > pagedList.TotalPages)
                    firstPageToDisplay = pagedList.TotalPages - maxPageNumbersToDisplay + 1;
            }

            if (Options.DisplayLinkToFirstPage == PaginationDisplayMode.Always || (Options.DisplayLinkToFirstPage == PaginationDisplayMode.IfNeeded && firstPageToDisplay > 1))
                listItemLinks.Add(PaginationHelper.First(pagedList, pageUrlGenerator, Options));

            if (Options.DisplayLinkToPreviousPage == PaginationDisplayMode.Always || (Options.DisplayLinkToPreviousPage == PaginationDisplayMode.IfNeeded && !pagedList.IsFirstPage))
                listItemLinks.Add(PaginationHelper.Previous(pagedList, pageUrlGenerator, Options));

            if (Options.DisplayLinkToIndividualPages)
            {
                if (Options.DisplayEllipsesWhenNotShowingAllPageNumbers && firstPageToDisplay > 1)
                    listItemLinks.Add(PaginationHelper.Ellipses(Options));

                foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
                {
                    if (i > firstPageToDisplay && !string.IsNullOrWhiteSpace(Options.DelimiterBetweenPageNumbers))
                        listItemLinks.Add(PaginationHelper.WrapInListItem(Options.DelimiterBetweenPageNumbers));

                    listItemLinks.Add(PaginationHelper.Page(i, pagedList, pageUrlGenerator, Options));
                }

                if (Options.DisplayEllipsesWhenNotShowingAllPageNumbers && (firstPageToDisplay + pageNumbersToDisplay - 1) < pagedList.TotalPages)
                    listItemLinks.Add(PaginationHelper.Ellipses(Options));
            }

            if (Options.DisplayLinkToNextPage == PaginationDisplayMode.Always || (Options.DisplayLinkToNextPage == PaginationDisplayMode.IfNeeded && !pagedList.IsLastPage))
                listItemLinks.Add(PaginationHelper.Next(pagedList, pageUrlGenerator, Options));

            if (Options.DisplayLinkToLastPage == PaginationDisplayMode.Always || (Options.DisplayLinkToLastPage == PaginationDisplayMode.IfNeeded && lastPageToDisplay < pagedList.TotalPages))
                listItemLinks.Add(PaginationHelper.Last(pagedList, pageUrlGenerator, Options));

            if (listItemLinks.Any())
            {
                if (!string.IsNullOrWhiteSpace(Options.FirstListItemCssClass))
                    listItemLinks.First().AddCssClass(Options.FirstListItemCssClass);

                if (!string.IsNullOrWhiteSpace(Options.LastListItemCssClass))
                    listItemLinks.Last().AddCssClass(Options.LastListItemCssClass);

                foreach (var li in listItemLinks)
                    foreach (var c in Options.LiElementCssClasses ?? Enumerable.Empty<string>())
                        li.AddCssClass(c);
            }

            var ul = new TagBuilder("ul");
            foreach (var listItemLink in listItemLinks)
                ul.InnerHtml.AppendHtml(listItemLink);
            foreach (var c in Options.UlElementCssClasses ?? Enumerable.Empty<string>())
                ul.AddCssClass(c);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            string classes = "pagination-container";
            if (Options.ContainerCssClasses != null && Options.ContainerCssClasses.Any())
            {
                classes += " " + string.Join(" ", Options.ContainerCssClasses);
            }
            output.Attributes.Add("class", classes);
            output.Content.AppendHtml(ul);
        }

        private string AppendPageNumberToPageUrl(int pageNumber)
        {
            string action = ViewContext.RouteData.Values["action"].ToString();
            string controller = ViewContext.RouteData.Values["controller"].ToString();
            string routeParamName = !string.IsNullOrEmpty(Options.PageNumberRouteParamName) ? Options.PageNumberRouteParamName : "page";
            var routeValues = ViewContext.HttpContext.Request.Query.ToRouteValueDictionary(routeParamName, pageNumber);
            return _urlHelper.Action(action, controller, routeValues);
        }
    }

    internal static class PaginationHelper
    {
        public static TagBuilder WrapInListItem(string text)
        {
            var li = new TagBuilder("li");
            li.InnerHtml.Append(text);
            return li;
        }

        public static TagBuilder WrapInListItem(TagBuilder inner, PaginationOptions options, params string[] classes)
        {
            var li = new TagBuilder("li");
            foreach (var @class in classes)
                li.AddCssClass(@class);
            if (options.FunctionToTransformEachPageLink != null)
                return options.FunctionToTransformEachPageLink(li, inner);
            li.InnerHtml.AppendHtml(inner);
            return li;
        }

        public static TagBuilder First(IPagedList pageData, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            const int targetPageNumber = 1;
            var first = new TagBuilder("a");
            first.InnerHtml.Append(string.Format(options.LinkToFirstPageFormat, targetPageNumber));

            if (pageData.IsFirstPage)
                return WrapInListItem(first, options, "pagination-first", options.LiElementDisabledCssClass);

            first.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(first, options, "pagination-first");
        }

        public static TagBuilder Previous(IPagedList pageData, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var targetPageNumber = pageData.PageNumber - 1;
            var previous = new TagBuilder("a");
            previous.InnerHtml.Append(string.Format(options.LinkToPreviousPageFormat, targetPageNumber));
            previous.Attributes["rel"] = "prev";

            if (!pageData.HasPreviousPage)
                return WrapInListItem(previous, options, "pagination-previous", options.LiElementDisabledCssClass);

            previous.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(previous, options, "pagination-previous");
        }

        public static TagBuilder Page(int i, IPagedList pageData, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var format = options.FunctionToDisplayEachPageNumber
                ?? (pageNumber => string.Format(options.LinkToIndividualPageFormat, pageNumber));
            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.InnerHtml.Append(format(targetPageNumber));

            if (i == pageData.PageNumber)
                return WrapInListItem(page, options, options.LiElementActiveCssClass);

            page.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(page, options);
        }

        public static TagBuilder Next(IPagedList pageData, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var targetPageNumber = pageData.PageNumber + 1;
            var next = new TagBuilder("a");
            next.InnerHtml.Append(string.Format(options.LinkToNextPageFormat, targetPageNumber));
            next.Attributes["rel"] = "next";

            if (!pageData.HasNextPage)
                return WrapInListItem(next, options, "pagination-next", options.LiElementDisabledCssClass);

            next.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(next, options, "pagination-next");
        }

        public static TagBuilder Last(IPagedList pageData, Func<int, string> generatePageUrl, PaginationOptions options)
        {
            var targetPageNumber = pageData.TotalPages;
            var last = new TagBuilder("a");
            last.InnerHtml.Append(string.Format(options.LinkToLastPageFormat, targetPageNumber));

            if (pageData.IsLastPage)
                return WrapInListItem(last, options, "pagination-last", options.LiElementDisabledCssClass);

            last.Attributes["href"] = generatePageUrl(targetPageNumber);
            return WrapInListItem(last, options, "pagination-last");
        }

        public static TagBuilder Ellipses(PaginationOptions options)
        {
            var a = new TagBuilder("a");
            a.InnerHtml.AppendHtml(options.EllipsesFormat);

            return WrapInListItem(a, options, "pagination-ellipses", options.LiElementDisabledCssClass);
        }
    }

    public enum PaginationDisplayMode
    {
        Always,
        Never,
        IfNeeded
    }

    public class PaginationOptions
    {
        public IPagedList PagedList { get; set; }
        public Func<int, string> PageUrlGenerator { get; set; }
        public IEnumerable<string> ContainerCssClasses { get; set; }
        public IEnumerable<string> UlElementCssClasses { get; set; }
        public IEnumerable<string> LiElementCssClasses { get; set; }
        public string FirstListItemCssClass { get; set; }
        public string LastListItemCssClass { get; set; }
        public string LiElementActiveCssClass { get; set; }
        public string LiElementDisabledCssClass { get; set; }
        public PaginationDisplayMode Display { get; set; }
        public PaginationDisplayMode DisplayLinkToFirstPage { get; set; }
        public PaginationDisplayMode DisplayLinkToLastPage { get; set; }
        public PaginationDisplayMode DisplayLinkToPreviousPage { get; set; }
        public PaginationDisplayMode DisplayLinkToNextPage { get; set; }
        public bool DisplayLinkToIndividualPages { get; set; }
        public int? MaximumPageNumbersToDisplay { get; set; }
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }
        public string EllipsesFormat { get; set; }
        public string LinkToFirstPageFormat { get; set; }
        public string LinkToPreviousPageFormat { get; set; }
        public string LinkToIndividualPageFormat { get; set; }
        public string LinkToNextPageFormat { get; set; }
        public string LinkToLastPageFormat { get; set; }
        public Func<int, string> FunctionToDisplayEachPageNumber { get; set; }
        public string DelimiterBetweenPageNumbers { get; set; }
        public Func<TagBuilder, TagBuilder, TagBuilder> FunctionToTransformEachPageLink { get; set; }
        public string PageNumberRouteParamName { get; set; }

        public PaginationOptions()
        {
            LiElementActiveCssClass = "active";
            LiElementDisabledCssClass = "disabled";
            DisplayLinkToFirstPage = PaginationDisplayMode.IfNeeded;
            DisplayLinkToLastPage = PaginationDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = PaginationDisplayMode.Never;
            DisplayLinkToNextPage = PaginationDisplayMode.Never;
            DisplayLinkToIndividualPages = true;
            MaximumPageNumbersToDisplay = 10;
            DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            EllipsesFormat = "…";
            LinkToFirstPageFormat = "««";
            LinkToPreviousPageFormat = "«";
            LinkToIndividualPageFormat = "{0}";
            LinkToNextPageFormat = "»";
            LinkToLastPageFormat = "»»";
            FunctionToDisplayEachPageNumber = null;
            FirstListItemCssClass = "first";
            LastListItemCssClass = "last";
            ContainerCssClasses = null;
            UlElementCssClasses = new[] { "pagination" };
            LiElementCssClasses = new[] { "page-item" };
            FunctionToTransformEachPageLink = (li, link) =>
            {
                link.AddCssClass("page-link");
                li.InnerHtml.AppendHtml(link);
                return li;
            };
            PageUrlGenerator = null;
            PageNumberRouteParamName = "page";
        }
    }
}
